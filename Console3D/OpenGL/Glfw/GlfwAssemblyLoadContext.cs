using com.RazorSoftware.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Text;

namespace Console3D.OpenGL.Glfw
{
    public sealed class GlfwAssemblyLoadContext : AssemblyLoadContext, IDisposable
    {
        public GlfwPlatformInfo PlatformInfo { get; }
        public string[] LibraryPaths { get; set; } = new string[] 
        { 
            "./glfw/${PLATFORM}/${ARCH}",
            "./glfw/${PLATFORM}",
            "./lib/glfw/${PLATFORM}/${ARCH}",
            "./lib/glfw/${PLATFORM}",
            "./lib/${PLATFORM}/${ARCH}",
            "./lib/${PLATFORM}",
        };

        private SlimLogger Logger { get; }

        private Dictionary<string, IntPtr> LoadedLibraryHandles { get; }

        public GlfwAssemblyLoadContext(string logFile = "./logs/native_dllloader.log")
        {
            PlatformInfo = new GlfwPlatformInfo();
            LoadedLibraryHandles = new Dictionary<string, IntPtr>(64, StringComparer.OrdinalIgnoreCase);

            if (!string.IsNullOrEmpty(logFile))
            {
                FileInfo fi = new FileInfo(logFile.Replace('/', Path.DirectorySeparatorChar));
                if (!fi.Directory.Exists)
                    fi.Directory.Create();

                Logger = new SlimLogger(fi.FullName, "DLLLOADER");

                //Logger.CloseStatistics = true;
                Logger.WrapRepetitions = true;
            }
        }

        public IntPtr LoadNativeGlfw()
        {
#if LOAD_GLES
            if (string.Equals(PlatformInfo.DetectedPlatformName, "windows", StringComparison.OrdinalIgnoreCase))
            {
                LoadNativeLibrary("d3dcompiler_46.dll");
                LoadNativeLibrary("libGLESv2.dll");
                LoadNativeLibrary("libEGL.dll");
            }
#endif
            return LoadNativeLibrary(PlatformInfo.NativeGlfwName);
        }

        private IntPtr LoadNativeLibrary(string libname)
        {
            libname = libname.ToLowerInvariant();

            if (Logger != null) Logger.ResetIndent();
            if (LoadedLibraryHandles.ContainsKey(libname))
            {
                if (Logger != null) Logger.WriteLine("Loading of Library %@ has been requested, but the library is already loaded. Returning from cache.", LogLevel.Debug, libname);
                return LoadedLibraryHandles[libname];
            }

            if (Logger != null)
            {
                Logger.WriteLine("Trying to load native library %@...", LogLevel.Debug, libname);
                Logger.IncreaseIndent();
                Logger.WriteLine("Current Directory: %@", LogLevel.Debug, new DirectoryInfo(Directory.GetCurrentDirectory()).FullName);
            }

            foreach (string dir in LibraryPaths)
            {
                string path = dir.Replace('/', Path.DirectorySeparatorChar);
                path = path.Replace("${PLATFORM}", PlatformInfo.DetectedPlatformName);
                path = path.Replace("${ARCH}", PlatformInfo.DetectedArchName);
                DirectoryInfo di = new DirectoryInfo(path.ToLowerInvariant());

                if (Logger != null) Logger.WriteLine("Trying path %@...", LogLevel.Debug, path);

                if (!di.Exists)
                {
                    if (Logger != null) Logger.WriteLine("External Native Lib directory '%@' cannot be found. Ignoring...", LogLevel.Debug, path);
                    continue;
                }

                if (Logger != null) Logger.Write("Searching %@... ", LogLevel.Debug, libname);

                FileInfo file = new FileInfo(Path.Combine(di.FullName, libname));

                if (!file.Exists)
                {
                    if (Logger != null) Logger.WriteLine("NOT FOUND.", LogLevel.Debug);
                    continue;
                }

                if (file.Length < 1)
                {
                    if (Logger != null) Logger.WriteLine("EMPTY.", LogLevel.Debug);
                    continue;
                }

                if (Logger != null) Logger.WriteLine("FOUND (%@ bytes). Trying to load...", LogLevel.Debug, file.Length.ToString("N0"));

                IntPtr result = LoadUnmanagedDllFromPath(file.FullName);

                if (result == IntPtr.Zero)
                {
                    if (Logger != null) Logger.WriteLine("Failed to load library %@ from path %@. No handle returned by the OS Loader.", LogLevel.Debug, libname, path);
                    continue;
                }

                if (Logger != null) Logger.WriteLine("Success loading library %@ from path %@. Load complete.", LogLevel.Debug, libname, path);

                LoadedLibraryHandles.Add(libname, result);
                return result;
            }

            if (Logger != null) Logger.WriteLine("Cannot load required native library with name %@ from any of the defined library paths. Module not found.", LogLevel.Error, libname);
            throw new FileNotFoundException("Cannot load a required unmanaged library: " + libname);
        }

        public void Dispose()
        {
            if (Logger != null) Logger.Dispose();
        }
    }
}
