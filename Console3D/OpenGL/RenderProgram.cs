using System;
using OpenToolkit.Graphics.OpenGL;
using Gl = OpenToolkit.Graphics.OpenGL.GL;

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
            GlVersion ver = GlVersion.Parse(Gl.GetString(StringName.Version));
            ContextCreationEventArgs args = new ContextCreationEventArgs(ver.Api, ver.Version, ver.ToString(), ver.Profile, Gl.GetString(StringName.Vendor));

            Gl.GetError();

            Renderer_ContextCreated(sender, args);
        }
        protected virtual void Renderer_ContextCreated(RenderThread sender, ContextCreationEventArgs args) { }

        protected virtual void Renderer_DrawEnd(RenderThread sender, FrameStageEventArgs args) { }

        protected abstract void Renderer_Draw(RenderThread sender, FrameStageEventArgs args);

        protected virtual void Renderer_DrawPrepare(RenderThread sender, FrameStageControllerEventArgs args) { }

        public static void CheckGlErrors(string stage)
        {
            ErrorCode code = Gl.GetError();

            //if (code == ErrorCode.NoError)
            //    throw new Exception("Eureka!");

            if (code != ErrorCode.NoError)
                throw new Exception(string.Format("OpenGL Draw error on stage {0}: {1}", stage, code.ToString()));
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

                    if (Renderer.TargetWindow.IsExiting || !Renderer.IsRunning)
                        break;

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
