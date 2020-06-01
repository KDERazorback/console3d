
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
