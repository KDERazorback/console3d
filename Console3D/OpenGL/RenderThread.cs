using GLFW;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Threading;
//using OpenGL;

namespace Console3D.OpenGL
{
    public class RenderThread
    {
        private ulong _lastFrameTimeValue = 0;
        private ulong _timeSinceLastFrame = 0; // Microseconds
        private ulong _frameIndex = 0;
        private ulong _totalFrameTime = 0; // Microseconds
        private ulong _currFrameTimerValue = 0;
        private string _initialWindowTitle = "OpenGL";
        private bool disposedValue;

        public delegate void ProcessingRawInputDelegate(RenderThread sender, RenderTimer timer);

        public event ProcessingRawInputDelegate ProcessingRawInput;


        public Thread Worker { get; private set; }
        private bool AbortFlag = false;
        public NativeWindow TargetWindow { get; private set; }
        public Size ViewportSize { get; private set; }
        public Color ClearColor { get; set; } = Color.CornflowerBlue;
        public Size WindowSize { get; private set; }
        public bool IsRunning => (Worker.IsAlive && Asynchronous) || (!Asynchronous && !AbortFlag);
        public bool VerticalSync { get; set; } = true;
        public ulong TimeSinceLastFrame => _timeSinceLastFrame;
        public ulong FrameIndex => _frameIndex;
        public bool Asynchronous { get; set; } = true;
        public bool Initialized { get; private set; } = false;
        public bool AutoEventPolling { get; set; } = true;
        public double SystemTimerResolution { get; private set; } // Microseconds
        private bool OwnsWindow { get; }
        public RenderTimer CurrentTime
        {
            get
            {
                return new RenderTimer(_frameIndex, _totalFrameTime, _timeSinceLastFrame);
            }
        }
        public string WindowTitle
        {
            get
            {
                if (TargetWindow == null || TargetWindow.Handle == IntPtr.Zero)
                    return _initialWindowTitle;

                return TargetWindow.Title;
            }
            set
            {
                if (TargetWindow == null || TargetWindow.Handle == IntPtr.Zero)
                {
                    _initialWindowTitle = value;
                    return;
                }

                TargetWindow.Title = value;
            }
        }

        public RenderThread(NativeWindow wnd, Size internalRes)
        {
            if (wnd == null || wnd.Handle == IntPtr.Zero)
                throw new ArgumentException("Invalid window specified.");

            OwnsWindow = false;
            TargetWindow = wnd;
        }

        public RenderThread(Size windowSize, Size internalRes)
        {
            OwnsWindow = true;
            WindowSize = windowSize;
        }

        public void Start()
        {
            if (!Initialized)
                throw new InvalidOperationException("The OpenGL Library bindings are not initialized.");

            if (Asynchronous)
                Worker.Start();
            else
                WorkerMain();
        }

        public void Stop()
        {
            AbortFlag = true;

            if (Asynchronous && Thread.CurrentThread.ManagedThreadId == Worker.ManagedThreadId)
                return;

            if (Asynchronous)
                Worker.Join();

            if (OwnsWindow)
            {
                GLFW.Glfw.DestroyWindow(TargetWindow);
                TargetWindow = null;
            }
        }

        private void WorkerMain()
        {
            if (!Initialized)
                throw new InvalidOperationException("The OpenGL Library bindings are not initialized.");

            if (TargetWindow == null)
                CreateMainWindow(WindowTitle);

            GLFW.Glfw.MakeContextCurrent(TargetWindow);

            if (!Gl.IsApiBound)
                Gl.BindApi();
            //Gl.BindAPI();

            ViewportSize = new Size(TargetWindow.ClientSize.Width, TargetWindow.ClientSize.Height);

            if (VerticalSync)
                GLFW.Glfw.SwapInterval(1);
            else
                GLFW.Glfw.SwapInterval(0);

            //Gl.ClearDepth(1.0f);
            //Gl.Enable(EnableCap.DepthTest);
            //Gl.DepthFunc(DepthFunction.Lequal);

            _lastFrameTimeValue = GLFW.Glfw.TimerValue;
            _timeSinceLastFrame = 0;
            _frameIndex = 0;

            _currFrameTimerValue = _lastFrameTimeValue;

            if (Asynchronous)
            {
                while (!GLFW.Glfw.WindowShouldClose(TargetWindow) && !AbortFlag)
                {
                    AdvanceFrame();
                }

                AbortFlag = true;
            }
        }

        public void AdvanceFrame()
        {
            if (_frameIndex > 0)
            {
                _currFrameTimerValue = GLFW.Glfw.TimerValue;
                _timeSinceLastFrame = (ulong)((_currFrameTimerValue - _lastFrameTimeValue) * SystemTimerResolution);
                _totalFrameTime += _timeSinceLastFrame;
            }

            ProcessInput();


            ClearBuffer();

            if (AutoEventPolling)
                ProcessEvents();

            SwapBuffers();

            SleepThread();

            _frameIndex++;
            _lastFrameTimeValue = _currFrameTimerValue;
        }

        private void ProcessInput()
        {
            OnProcessingRawInput(CurrentTime);
        }

        private void ClearBuffer()
        {
            //Gl.LoadIdentity();
            Gl.ClearColor(ClearColor.R / 255.0f, ClearColor.G / 255.0f, ClearColor.B / 255.0f, 0.5f);
            //glClearColor(ClearColor.R / 255.0f, ClearColor.G / 255.0f, ClearColor.B / 255.0f, 1.0f);
            Gl.Clear((int)global::OpenGL.ClearBufferMask.ColorBufferBit);
            //glClear((int)ClearBufferMask.ColorBufferBit);
        }

        private void SwapBuffers()
        {
            GLFW.Glfw.SwapBuffers(TargetWindow);
        }

        private void SleepThread()
        {

        }

        public void CreateMainWindow(string title)
        {
            if (TargetWindow != null)
                throw new InvalidOperationException("There is already a window created for the current Render thread.");

            TargetWindow = new NativeWindow(WindowSize.Width, WindowSize.Height, title);
        }

        public void CreateMainWindow()
        {
            CreateMainWindow(WindowTitle);
        }

        public void ProcessEvents()
        {
            GLFW.Glfw.PollEvents();
        }

        public void Shutdown()
        {
            GLFW.Glfw.Terminate();
            Initialized = false;
        }

        public void Initialize()
        {
            SystemTimerResolution = 1000000d / GLFW.Glfw.TimerFrequency;

            Worker = new Thread(WorkerMain);
            Worker.IsBackground = true;
            Worker.Name = "RenderThread";

            Initialized = true;
        }

        public void ProbeWindowSystem()
        {
            NativeWindow wnd = new NativeWindow(400, 300, "GLFW: Probe Window. === Loading... ===");
            wnd.Dispose();
        }

        public bool IsMainThread()
        {
            Thread t = Thread.CurrentThread;
            bool result = (t.IsAlive && !t.IsBackground && !t.IsThreadPoolThread);

            if (!result)
                return false;

            MethodInfo entryPoint = Assembly.GetEntryAssembly().EntryPoint;

            if (entryPoint == null)
                return true; // Cannot reliably determine if we are the main thread. Asume yes.

            StackTrace trace = new StackTrace();

            if (trace.GetFrame(trace.FrameCount - 1).GetMethod() == entryPoint) // The thread started on the entry point. We are probably the main thread.
                return true;

            return false;
        }

        ~RenderThread()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (OwnsWindow && TargetWindow != null && TargetWindow.Handle != IntPtr.Zero)
                    {
                        GLFW.Glfw.DestroyWindow(TargetWindow);
                        TargetWindow = null;
                    }
                }

                Shutdown();
                disposedValue = true;
            }
        }

        protected virtual void OnProcessingRawInput(RenderTimer timer)
        {
            ProcessingRawInput?.Invoke(this, timer);
        }
    }
}
