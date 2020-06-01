using OpenToolkit.Windowing.Desktop;

namespace Console3D.OpenGL
{
    public class RenderWindow : NativeWindow
    {
        protected int _swapInterval = 1;

        public RenderWindow(NativeWindowSettings settings) : base(settings)
        {
        }

        public void SwapBuffers()
        {
            unsafe
            {
                OpenToolkit.Windowing.GraphicsLibraryFramework.GLFW.SwapBuffers(WindowPtr);
            }
        }

        public int SwapInterval
        {
            get
            {
                return _swapInterval;
            }
            set
            {
                OpenToolkit.Windowing.GraphicsLibraryFramework.GLFW.SwapInterval(value);
                _swapInterval = value;
            }
        }
    }
}
