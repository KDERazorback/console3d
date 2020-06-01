using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Threading;
using Console3D.OpenGL.Shaders;
using OpenToolkit.Windowing.Desktop;
using OpenToolkit.Graphics.OpenGL;
using Gl = OpenToolkit.Graphics.OpenGL.GL;
using System.Runtime.InteropServices;
using OpenToolkit.Windowing.GraphicsLibraryFramework;

namespace Console3D.OpenGL
{
    public class RenderThread
    {
        private ulong _lastFrameTimeValue = 0;
        private ulong _timeSinceLastFrame = 0; // Microseconds
        private ulong _frameIndex = 0;
        private ulong _framesSkipped = 0;
        private ulong _totalFrameTime = 0; // Microseconds
        private ulong _currFrameTimerValue = 0;
        private string _initialWindowTitle = "OpenGL";
        private bool disposedValue;
        private Size _internalResolution = new Size(800, 600);
        private bool _fullscreen = false;
        private bool _wireframeMode = false;

        public delegate void FrameStageEventDelegate(RenderThread sender, FrameStageEventArgs args);
        public delegate void FrameStageControllerEventDelegate(RenderThread sender, FrameStageControllerEventArgs args);
        public delegate void ContextEventDelegate(RenderThread sender);

        public event FrameStageControllerEventDelegate ProcessingRawInput;
        public event FrameStageEventDelegate FrameStart;
        public event FrameStageEventDelegate FrameEnd;
        public event FrameStageEventDelegate BufferClear;
        public event FrameStageEventDelegate BufferSwap;
        public event FrameStageEventDelegate Draw;
        public event FrameStageEventDelegate DrawEnd;
        public event FrameStageControllerEventDelegate DrawPrepare;
        public event FrameStageEventDelegate Sleeping;
        public event ContextEventDelegate ContextCreated;


        public Thread Worker { get; private set; }
        private bool AbortFlag = false;
        public RenderWindow TargetWindow { get; private set; }
        public Size ViewportSize { get; private set; }
        public Color ClearColor { get; set; } = Color.CornflowerBlue;
        public Size WindowSize { get; private set; }
        public bool IsRunning => (Worker != null) && ((Worker.IsAlive && Asynchronous) || (!Asynchronous && !AbortFlag));
        public bool VerticalSync { get; set; } = true;
        public ulong TimeSinceLastFrame => _timeSinceLastFrame;
        public ulong FrameIndex => _frameIndex;
        public bool Asynchronous { get; set; } = true;
        public bool Initialized { get; private set; } = false;
        public bool AutoEventPolling { get; set; } = true;
        public double SystemTimerResolution { get; private set; } // Microseconds
        private bool OwnsWindow { get; }
        public bool AutoClearFrames { get; set; } = true;
        public bool AutoSetViewport { get; set; } = true;
        public AutoEnableCapabilitiesFlags AutoEnableCaps { get; set; } = AutoEnableCapabilitiesFlags.All;
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
                if (TargetWindow == null)
                    return _initialWindowTitle;

                return TargetWindow.Title;
            }
            set
            {
                if (TargetWindow == null)
                {
                    _initialWindowTitle = value;
                    return;
                }

                TargetWindow.Title = value;
            }
        }
        public Size InternalResolution
        {
            get
            {
                return _internalResolution;
            }
            set
            {
                if (IsRunning)
                    throw new InvalidOperationException("Cannot modify internal resolution while the Render thread is running.");

                if (value == null)
                    throw new ArgumentNullException("Internal resolution value cannot be null.");

                if (value.IsEmpty)
                    throw new ArgumentOutOfRangeException("Invalid internal resolution specified. Value cannot be empty.");

                _internalResolution = value;
            }
        }
        public bool Fullscreen
        {
            get
            {
                return _fullscreen;
            }
            set
            {
                _fullscreen = value;
                TargetWindow.IsFullscreen = value;
            }
        }
        public bool FrameStarted { get; private set; }
        public ShaderProgram ActiveShaderProgram { get; private set; }
        public bool WireframeMode
        {
            get
            {
                return _wireframeMode;
            }
            set
            {
                if (TargetWindow != null)
                {
                    if (value)
                        Gl.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                    else
                        Gl.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                }

                _wireframeMode = value;
            }
        }

        public RenderThread(RenderWindow wnd, Size internalRes)
        {
            if (wnd == null)
                throw new ArgumentException("Invalid window specified.");

            OwnsWindow = false;
            TargetWindow = wnd;
            InternalResolution = internalRes;
        }

        public RenderThread(Size windowSize, Size internalRes)
        {
            if (windowSize == null)
                throw new ArgumentNullException("Internal resolution value cannot be null.");
            if (windowSize.IsEmpty)
                throw new ArgumentOutOfRangeException("Window size cannot be empty.");

            OwnsWindow = true;
            WindowSize = windowSize;
            InternalResolution = internalRes;
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
                TargetWindow.Dispose();
                TargetWindow = null;
            }
        }

        private void WorkerMain()
        {
            if (!Initialized)
                throw new InvalidOperationException("The OpenGL Library bindings are not initialized.");

            if (TargetWindow == null)
                CreateMainWindow(WindowTitle);

            TargetWindow.MakeCurrent();

            ViewportSize = new Size(TargetWindow.ClientSize.X, TargetWindow.ClientSize.Y);

            if (WireframeMode)
                WireframeMode = true; // Recall

            if (AutoSetViewport)
                Gl.Viewport(0, 0, ViewportSize.Width, ViewportSize.Height);


            if (VerticalSync)
                TargetWindow.SwapInterval = 1;
            else
                TargetWindow.SwapInterval = 0;

            if (AutoEnableCaps.HasFlag(AutoEnableCapabilitiesFlags.Blend))
                Gl.Enable(EnableCap.Blend);

            OnContextCreated();

            _lastFrameTimeValue = 0; // TODO: Set here a timer value!
            _timeSinceLastFrame = 0;
            _frameIndex = 0;

            _currFrameTimerValue = _lastFrameTimeValue;

            if (Asynchronous)
            {
                while (!TargetWindow.IsExiting && !AbortFlag)
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
                _currFrameTimerValue = 0; // TODO: Set here a timer value!
                _timeSinceLastFrame = (ulong)((_currFrameTimerValue - _lastFrameTimeValue) * SystemTimerResolution);
                _totalFrameTime += _timeSinceLastFrame;
            }

            FrameStageControllerEventArgs args = new FrameStageControllerEventArgs(CurrentTime);

            FrameStarted = true;
            OnFrameStart(CurrentTime);

            ProcessInput(args);

            if (args.AbortExecution)
            {
                AbortFlag = true;
                return;
            }

            if (AutoClearFrames)
            {
                ClearBuffer();

                OnBufferClear(CurrentTime);
            }

            if (AutoEventPolling)
                ProcessEvents();

            args = new FrameStageControllerEventArgs(CurrentTime);
            OnDrawPrepare(args);

            if (args.AbortExecution)
            {
                AbortFlag = true;
                return;
            }
            if (args.SkipFrame)
            {
                _frameIndex++;
                _lastFrameTimeValue = _currFrameTimerValue;
                _framesSkipped++;
                return;
            }

            OnDraw(CurrentTime);
            OnDrawEnd(CurrentTime);
            FrameStarted = false;

            SwapBuffers();

            OnBufferSwap(CurrentTime);

            OnFrameEnd(CurrentTime);
            ActiveShaderProgram = null;

            SleepThread();

            _frameIndex++;
            _lastFrameTimeValue = _currFrameTimerValue;
        }

        private void ProcessInput(FrameStageControllerEventArgs args)
        {
            OnProcessingRawInput(args);
        }

        private void ClearBuffer()
        {
            Gl.ClearColor(ClearColor.R / 255.0f, ClearColor.G / 255.0f, ClearColor.B / 255.0f, 0.5f);
            Gl.Clear(ClearBufferMask.ColorBufferBit);
        }

        private void SwapBuffers()
        {
            TargetWindow.SwapBuffers();
        }

        private void SleepThread()
        {
            OnSleeping(CurrentTime);
        }

        public void CreateMainWindow(string title)
        {
            if (TargetWindow != null)
                throw new InvalidOperationException("There is already a window created for the current Render thread.");

            NativeWindowSettings settings = new NativeWindowSettings() { IsFullscreen = Fullscreen, StartVisible = true, StartFocused = true, Title = title, Size = new OpenToolkit.Mathematics.Vector2i(WindowSize.Width, WindowSize.Height) };

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                settings.API = OpenToolkit.Windowing.Common.ContextAPI.OpenGL;
                settings.APIVersion = new Version(3, 3, 0, 0);
                settings.Flags |= OpenToolkit.Windowing.Common.ContextFlags.ForwardCompatible;
                settings.Profile = OpenToolkit.Windowing.Common.ContextProfile.Core;
            }
            TargetWindow = new RenderWindow(settings);
        }

        public void CreateMainWindow()
        {
            CreateMainWindow(WindowTitle);
        }

        public void ProcessEvents()
        {
            TargetWindow.ProcessEvents();
        }

        public void Shutdown()
        {
            Initialized = false;
        }

        public void Initialize()
        {
            SystemTimerResolution = 1000000d / 10000000d; // TODO: Set here the timer resolution value!

            Worker = new Thread(WorkerMain);
            Worker.IsBackground = true;
            Worker.Name = "RenderThread";

            Initialized = true;
        }

        public void ProbeWindowSystem(bool display = false)
        {
            NativeWindowSettings settings = new NativeWindowSettings() { IsFullscreen = Fullscreen, Size = new OpenToolkit.Mathematics.Vector2i(800, 600), StartVisible = false, StartFocused = false, Title = "GLFW: Probe Window. === Loading... ===" };
            NativeWindow wnd = new NativeWindow(settings);
            if (display)
                wnd.IsVisible = true;
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

        public void SelectShader(ShaderProgram program)
        {
            if (!FrameStarted)
                throw new InvalidOperationException("Cannot select a Shader program when no frame is currently started on the GPU.");

            ActiveShaderProgram = program;
            Gl.UseProgram(program.ProgramId);
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

        public void SetUniform(string name, float v1, float? v2 = null, float? v3 = null, float? v4 = null)
        {
            if (ActiveShaderProgram == null)
                throw new InvalidOperationException("No shader program is loaded. Cannot set uniforms.");

            int uniformLocation = Gl.GetUniformLocation(ActiveShaderProgram.ProgramId, name);

            if (uniformLocation < 0)
                throw new KeyNotFoundException(string.Format("The specified uniform '{0}' cannot be found for program '{1}'.", name, ActiveShaderProgram.ProgramId));

            if (v4 != null && v4.HasValue)
            {
                Gl.Uniform4(uniformLocation, v1, v2.Value, v3.Value, v4.Value);
                return;
            }

            if (v3 != null && v3.HasValue)
            {
                Gl.Uniform3(uniformLocation, v1, v2.Value, v3.Value);
                return;
            }

            if (v2 != null && v2.HasValue)
            {
                Gl.Uniform2(uniformLocation, v1, v2.Value);
                return;
            }

            Gl.Uniform1(uniformLocation, v1);
            return;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (OwnsWindow && TargetWindow != null)
                    {
                        TargetWindow.Dispose();
                        TargetWindow = null;
                    }
                }

                Shutdown();
                disposedValue = true;
            }
        }

        protected virtual void OnProcessingRawInput(FrameStageControllerEventArgs args)
        {
            ProcessingRawInput?.Invoke(this, args);
        }

        protected virtual void OnFrameStart(RenderTimer timer)
        {
            FrameStart?.Invoke(this, new FrameStageEventArgs(timer));
        }

        protected virtual void OnFrameEnd(RenderTimer timer)
        {
            FrameEnd?.Invoke(this, new FrameStageEventArgs(timer));
        }

        protected virtual void OnBufferClear(RenderTimer timer)
        {
            BufferClear?.Invoke(this, new FrameStageEventArgs(timer));
        }

        protected virtual void OnBufferSwap(RenderTimer timer)
        {
            BufferSwap?.Invoke(this, new FrameStageEventArgs(timer));
        }

        protected virtual void OnDraw(RenderTimer timer)
        {
            Draw?.Invoke(this, new FrameStageEventArgs(timer));
        }

        protected virtual void OnDrawEnd(RenderTimer timer)
        {
            DrawEnd?.Invoke(this, new FrameStageEventArgs(timer));
        }

        protected virtual void OnDrawPrepare(FrameStageControllerEventArgs args)
        {
            DrawPrepare?.Invoke(this, args);
        }

        protected virtual void OnSleeping(RenderTimer timer)
        {
            Sleeping?.Invoke(this, new FrameStageEventArgs(timer));
        }

        protected virtual void OnContextCreated()
        {
            ContextCreated?.Invoke(this);
        }
    }
}
