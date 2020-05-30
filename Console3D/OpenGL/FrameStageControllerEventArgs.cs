using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D.OpenGL
{
    public class FrameStageControllerEventArgs : FrameStageEventArgs
    {
        public FrameStageControllerEventArgs(RenderTimer currentTime) : base(currentTime)
        {
        }

        public bool SkipFrame { get; set; }
        public bool AbortExecution { get; set; }
    }
}
