using System;
using System.IO;
using System.Text;
#if !EMBEDDED_GL
using OpenGL;
#endif
namespace Console3D.OpenGL.Shaders
{
    public class ShaderProgram : IDisposable
    {
        public uint ProgramId { get; protected set; }
        public string VertexShaderCode { get; }
        public string FragmentShaderCode { get; }
        public bool Compiled { get; protected set; }

        public ShaderProgram(string vertexShader, string fragmentShader)
        {
            if (string.IsNullOrWhiteSpace(vertexShader))
                throw new ArgumentNullException("VertexShader code cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(fragmentShader))
                throw new ArgumentNullException("FragmentShader code cannot be null or empty.");

            VertexShaderCode = vertexShader.Trim();
            FragmentShaderCode = fragmentShader.Trim();

            if (VertexShaderCode == null || VertexShaderCode.Length < 1)
                throw new ArgumentException("The VertexShader code is empty or cannot be parsed.");
            if (FragmentShaderCode == null || FragmentShaderCode.Length < 1)
                throw new ArgumentException("The FragmentShader code is empty or cannot be parsed.");
        }

        public uint Compile()
        {
            if (Compiled)
                throw new InvalidOperationException("Cannot compile shader because its aready compiled and loaded into the GPU.");

            uint vertexId = 0;
            uint fragmentId = 0;
            uint programId = 0;

            try
            {
#if EMBEDDED_GL
                vertexId = CompileShaderSource((int)ShaderType.VertexShader, VertexShaderCode);
                fragmentId = CompileShaderSource((int)ShaderType.FragmentShader, FragmentShaderCode);
#else
                vertexId = CompileShaderSource(ShaderType.VertexShader, VertexShaderCode);
                fragmentId = CompileShaderSource(ShaderType.FragmentShader, FragmentShaderCode);
#endif

                programId = LinkShaderProgram(vertexId, fragmentId);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (vertexId > 0) DestroyShader(vertexId);
                if (fragmentId > 0) DestroyShader(fragmentId);
            }

            ProgramId = programId;
            Compiled = true;

            return programId;
        }

#if EMBEDDED_GL
        protected uint CompileShaderSource(int t, string code)
        {
            uint shaderId = Gl.CreateShader(t);
            Gl.ShaderSource(shaderId, new string[] { code });
            Gl.CompileShader(shaderId);
            Gl.GetShader(shaderId, (int)ShaderParameterName.CompileStatus, out int success);
            if (success < 1)
            {
                string infoLog = null;
                Gl.GetShader(shaderId, (int)ShaderParameterName.InfoLogLength, out int loglen);
                if (loglen > 0)
                {
                    StringBuilder log = new StringBuilder(loglen);
                    Gl.GetShaderInfoLog(shaderId, loglen, out _, log);
                }
                throw new ShaderException(this, "Shader compilation failed. Type:" + t.ToString(), infoLog);
            }

            return shaderId;
        }
#else
        protected uint CompileShaderSource(ShaderType t, string code)
        {
            uint shaderId = Gl.CreateShader(t);
            Gl.ShaderSource(shaderId, new string[] { code });
            Gl.CompileShader(shaderId);
            Gl.GetShader(shaderId, ShaderParameterName.CompileStatus, out int success);
            if (success < 1)
            {
                string infoLog = null;
                Gl.GetShader(shaderId, ShaderParameterName.InfoLogLength, out int loglen);
                if (loglen > 0)
                {
                    StringBuilder log = new StringBuilder(loglen);
                    Gl.GetShaderInfoLog(shaderId, loglen, out _, log);
                    infoLog = log.ToString();
                }
                throw new ShaderException(this, "Shader compilation failed. Type:" + t.ToString(), infoLog);
            }

            return shaderId;
        }
#endif

#if EMBEDDED_GL
        protected uint LinkShaderProgram(uint vertexId, uint fragmentId)
        {
            uint programId = Gl.CreateProgram();
            Gl.AttachShader(programId, vertexId);
            Gl.AttachShader(programId, fragmentId);
            Gl.LinkProgram(programId);

            Gl.GetProgram(programId, (int)ProgramProperty.LinkStatus, out int success);
            if (success < 1)
            {
                string infoLog = null;
                Gl.GetProgram(programId, (int)ProgramProperty.InfoLogLength, out int loglen);
                if (loglen > 0)
                {
                    StringBuilder log = new StringBuilder(loglen);
                    Gl.GetProgramInfoLog(programId, loglen, out _, log);
                    infoLog = log.ToString();
                }
                throw new ShaderException(this, "Program Link failed.", infoLog);
            }

            return programId;
        }
#else
        protected uint LinkShaderProgram(uint vertexId, uint fragmentId)
        {
            uint programId = Gl.CreateProgram();
            Gl.AttachShader(programId, vertexId);
            Gl.AttachShader(programId, fragmentId);
            Gl.LinkProgram(programId);

            Gl.GetProgram(programId, ProgramProperty.LinkStatus, out int success);
            if (success < 1)
            {
                string infoLog = null;
                Gl.GetProgram(programId, ProgramProperty.InfoLogLength, out int loglen);
                if (loglen > 0)
                {
                    StringBuilder log = new StringBuilder(loglen);
                    Gl.GetProgramInfoLog(programId, loglen, out _, log);
                    infoLog = log.ToString();
                }
                throw new ShaderException(this, "Program Link failed.", infoLog);
            }

            return programId;
        }
#endif

        protected void DestroyShader(uint shaderId)
        {
            Gl.DeleteShader(shaderId);
        }

        public void Unload()
        {
            if (!Compiled)
                throw new InvalidOperationException("Cannot unload shader because its not loaded into the GPU.");

            Compiled = false;
            Gl.DeleteProgram(ProgramId);
        }

        public void Dispose()
        {
            if (Compiled)
                Unload();
        }

        public static ShaderProgram FromFiles(string vertexShader, string fragmentShader)
        {
            vertexShader = vertexShader.Replace('/', Path.DirectorySeparatorChar);
            fragmentShader = fragmentShader.Replace('/', Path.DirectorySeparatorChar);

            string vsh = null;
            string fsh = null;

            using (FileStream fs = new FileStream(vertexShader, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (StreamReader reader = new StreamReader(fs))
                vsh = reader.ReadToEnd();

            using (FileStream fs = new FileStream(fragmentShader, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (StreamReader reader = new StreamReader(fs))
                fsh = reader.ReadToEnd();

            return new ShaderProgram(vsh, fsh);
        }
    }
}
