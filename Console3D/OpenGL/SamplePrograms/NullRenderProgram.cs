using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D.OpenGL.SamplePrograms
{
    public class NullRenderProgram : RenderProgram
    {
        public NullRenderProgram(RenderThread renderer) : base(renderer)
        {
            Renderer.AutoClearFrames = true;
            Renderer.AutoEnableCaps = AutoEnableCapabilitiesFlags.None;
        }

        protected override void Renderer_Draw(RenderThread sender, FrameStageEventArgs args)
        {
            // Do nothing
        }
    }
}
