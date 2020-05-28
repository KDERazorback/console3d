using com.RazorSoftware.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using System.Text;

namespace Console3D.OpenGL.Glfw
{
    public static class LibraryLoadManager
    {
        public static bool Ready { get; private set; }
        public static GlfwAssemblyLoadContext AssemblyLoader { get; private set; }

        public static void Initialize(string logFile = "./logs/native_dllloader.log")
        {
            AssemblyLoader = new GlfwAssemblyLoadContext(logFile);
            AssemblyLoadContext.Default.ResolvingUnmanagedDll += AssemblyResolver_ResolvingUnmanagedDll;
            AssemblyLoadContext.Default.Resolving += AssemblyResolver_Resolving;
            Ready = true;
        }

        private static System.Reflection.Assembly AssemblyResolver_Resolving(AssemblyLoadContext arg1, System.Reflection.AssemblyName arg2)
        {
            throw new NotImplementedException();
        }

        public static void Shutdown()
        {
            Ready = false;
            AssemblyLoadContext.Default.ResolvingUnmanagedDll -= AssemblyResolver_ResolvingUnmanagedDll;
        }

        private static IntPtr AssemblyResolver_ResolvingUnmanagedDll(System.Reflection.Assembly arg1, string arg2)
        {
            if (string.Equals(arg2, "glfw", StringComparison.OrdinalIgnoreCase))
                return AssemblyLoader.LoadNativeGlfw();

            throw new System.IO.FileNotFoundException(string.Format("The specified unmanaged library {0} cannot be found", arg2));
        }
    }
}
