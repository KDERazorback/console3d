using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D.OpenGL
{
    public readonly struct RenderTimer
    {
        public readonly ulong FrameIndex;
        public readonly ulong TotalRuntime;
        public readonly ulong DeltaRuntime;

        public RenderTimer(ulong frameIndex, ulong totalRuntime, ulong deltaRuntime)
        {
            FrameIndex = frameIndex;
            TotalRuntime = totalRuntime;
            DeltaRuntime = deltaRuntime;
        }
    }
}
