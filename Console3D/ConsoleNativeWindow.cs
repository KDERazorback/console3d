using OpenToolkit.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D
{
    public class ConsoleNativeWindow : NativeWindow
    {
        public ConsoleNativeWindow(NativeWindowSettings settings) : base(settings)
        {
        }

        public void SwapBuffers()
        {
            unsafe
            {
                OpenToolkit.Windowing.GraphicsLibraryFramework.GLFW.SwapBuffers(this.WindowPtr);
            }
        }
    }
}
