using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
#if !EMBEDDED_GL
using global::OpenGL;
using Gl = global::OpenGL.Gl;
#endif

namespace Console3D.OpenGL.SamplePrograms
{
    public class TriangleRenderProgram : RenderProgram
    {
        public TriangleRenderProgram(RenderThread renderer) : base(renderer)
        {
            Renderer.AutoEnableCaps = AutoEnableCapabilitiesFlags.None;
            Renderer.AutoClearFrames = false;
        }

        protected override void Renderer_ContextCreated(RenderThread sender, ContextCreationEventArgs args)
        {
            uint vao = Gl.GenBuffer();
            Gl.BindVertexArray(vao);
        }

        protected override void Renderer_Draw(RenderThread sender, FrameStageEventArgs args)
        {
            float frameTime = 1 - (args.CurrentTime.FrameIndex / 1000.0f);
            if (frameTime < 0)
                frameTime = 0;
            if (frameTime > 1)
                frameTime = 1;
            Gl.ClearColor((Renderer.ClearColor.R / 255.0f) * frameTime, (Renderer.ClearColor.G / 255.0f), (Renderer.ClearColor.B / 255.0f) * frameTime, 1.0f);

            Gl.Clear(ClearBufferMask.ColorBufferBit);

            Gl.Begin(PrimitiveType.Triangles);
            Gl.Color3(1.0f, 0.0f, 0.0f); Gl.Vertex2(0.0f, 0.0f);
            Gl.Color3(0.0f, 1.0f, 0.0f); Gl.Vertex2(0.5f, 1.0f);
            Gl.Color3(0.0f, 0.0f, 1.0f); Gl.Vertex2(1.0f, 0.0f);
            Gl.End();
        }
    }
}
