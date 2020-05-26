using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D.OpenGL
{
    public class FrameStageEventArgs
    {
        public FrameStageEventArgs(RenderTimer currentTime)
        {
            CurrentTime = currentTime;
        }

        public RenderTimer CurrentTime { get; }
    }
}
