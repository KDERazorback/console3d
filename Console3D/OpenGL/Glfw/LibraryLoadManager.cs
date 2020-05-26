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
            Ready = true;
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
