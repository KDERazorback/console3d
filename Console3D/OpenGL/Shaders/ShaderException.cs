using System;
namespace Console3D.OpenGL.Shaders
{
    public class ShaderException : Exception
    {
        public string InfoLog { get; }
        public ShaderProgram Program { get; }

        public ShaderException(ShaderProgram program, string message, string infoLog) : base(message)
        {
            InfoLog = infoLog;
            Program = program;
        }
    }
}
