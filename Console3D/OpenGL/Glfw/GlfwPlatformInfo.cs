using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Console3D.OpenGL.Glfw
{
    public class GlfwPlatformInfo
    {
        public string DetectedPlatformName { get; }
        public string DetectedArchName { get; }
        public string NativeGlfwName { get; }

        public GlfwPlatformInfo()
        {
            DetectedPlatformName = GetPlatformName();
            DetectedArchName = GetArchName();
            NativeGlfwName = GetNativeGlfwName();
        }

        private static string GetPlatformName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return "Windows";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return "Linux";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return "MacOSX";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
                return "FreeBSD";

            return "Unknown";
        }

        private static string GetArchName()
        {
            if (RuntimeInformation.OSArchitecture == Architecture.X86)
                return "x86";
            if (RuntimeInformation.OSArchitecture == Architecture.X64)
                return "x64";
            if (RuntimeInformation.OSArchitecture == Architecture.Arm)
                return "arm";
            if (RuntimeInformation.OSArchitecture == Architecture.Arm64)
                return "arm64";

            return "Unknown";
        }

        private static string GetNativeGlfwName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return "Glfw3.dll";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return "glfw";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return "libglfw.3.dylib";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
                return "glfw";

            throw new PlatformNotSupportedException("The current platform is not supported by GLFW. Cannot guess library name by the current platform.");
        }

        public bool Is64Bits()
        {
            return (RuntimeInformation.OSArchitecture == Architecture.X64);
        }

        public bool IsIntelX86()
        {
            return (RuntimeInformation.OSArchitecture == Architecture.X64 || RuntimeInformation.OSArchitecture == Architecture.X86);
        }

        public bool IsArm()
        {
            return (RuntimeInformation.OSArchitecture == Architecture.Arm || RuntimeInformation.OSArchitecture == Architecture.Arm64);
        }
    }
}
