using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
#if !EMBEDDED_GL
using global::OpenGL;
using Gl = global::OpenGL.Gl;
#endif

namespace Console3D.OpenGL
{
    public abstract class RenderProgram : IDisposable
    {
        public RenderThread Renderer { get; }

        public delegate void RenderProgramEventDelegate(RenderProgram sender, RenderProgramEventArgs args);

        public event RenderProgramEventDelegate SyncRenderStart;
        public event RenderProgramEventDelegate SyncRenderEnd;

        public RenderProgram(RenderThread renderer)
        {
            Renderer = renderer;

            renderer.DrawPrepare += Renderer_DrawPrepare;
            renderer.Draw += Renderer_Draw;
            renderer.DrawEnd += Renderer_DrawEnd;
            renderer.ContextCreated += Renderer_ContextCreated_Wrap;
            renderer.ProcessingRawInput += Renderer_ProcessingRawInput;
        }

        protected virtual void Renderer_ProcessingRawInput(RenderThread sender, FrameStageControllerEventArgs args) { }

        protected virtual void Renderer_ContextCreated_Wrap(RenderThread sender)
        {
#if EMBEDDED_GL
            if (!OpenGL.Gl.IsApiBound)
                OpenGL.Gl.BindApi();
            KhronosVersion ver = Gl.CurrentVersion;
            ContextCreationEventArgs args = new ContextCreationEventArgs(ver.Api, new Version(ver.Major, ver.Minor, ver.Revision), ver.ToString(), ver.Profile, Gl.CurrentVendor);
#else
            Khronos.KhronosVersion ver = Gl.CurrentVersion;
            ContextCreationEventArgs args = new ContextCreationEventArgs(ver.Api, new Version(ver.Major, ver.Minor, ver.Revision), ver.ToString(), ver.Profile, Gl.CurrentVendor);
#endif


            Renderer_ContextCreated(sender, args);
        }
        protected virtual void Renderer_ContextCreated(RenderThread sender, ContextCreationEventArgs args) { }

        protected virtual void Renderer_DrawEnd(RenderThread sender, FrameStageEventArgs args) { }

        protected abstract void Renderer_Draw(RenderThread sender, FrameStageEventArgs args);

        protected virtual void Renderer_DrawPrepare(RenderThread sender, FrameStageControllerEventArgs args) { }

        protected static void CheckGlErrors(string stage)
        {
            ErrorCode code = Gl.GetError();

            if (code != ErrorCode.NoError)
                throw new System.Exception(string.Format("OpenGL Draw error on stage {0}: {1}", stage, code.ToString()));
        }

        protected virtual void LoadTextures() { }

        public virtual void Run()
        {
            Renderer.Start();

            LoadTextures();

            if (!Renderer.Asynchronous)
            {
                bool AbortMainLoop = false;
                RenderProgramEventArgs args = new RenderProgramEventArgs();
                while (!AbortMainLoop)
                {
                    OnSyncRenderStart(args);

                    if (args.AbortExecution)
                        break;

                    Renderer.ProcessEvents();
                    Renderer.AdvanceFrame();
                    OnSyncRenderEnd(args);

                    if (args.AbortExecution)
                        break;
                }
            }
        }

        protected virtual void OnSyncRenderStart(RenderProgramEventArgs e)
        {
            SyncRenderStart?.Invoke(this, e);
        }

        protected virtual void OnSyncRenderEnd(RenderProgramEventArgs e)
        {
            SyncRenderEnd?.Invoke(this, e);
        }

        public virtual void Stop()
        {
            Renderer.Stop();
        }

        public void Dispose()
        {
            Renderer.Shutdown();
            Renderer.Dispose();
        }
    }
}
