﻿using System;
using System.IO;
using System.Text;
#if !EMBEDDED_GL
using OpenToolkit.Graphics.OpenGL;
using Gl = OpenToolkit.Graphics.OpenGL.GL;
#endif

namespace Console3D.OpenGL.Shaders
{
    public class ShaderProgram : IDisposable
    {
        public int ProgramId { get; protected set; }
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

        public int Compile()
        {
            if (Compiled)
                throw new InvalidOperationException("Cannot compile shader because its aready compiled and loaded into the GPU.");

            int vertexId = 0;
            int fragmentId = 0;
            int programId = 0;

            try
            {
                vertexId = CompileShaderSource(ShaderType.VertexShader, VertexShaderCode);
                fragmentId = CompileShaderSource(ShaderType.FragmentShader, FragmentShaderCode);
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

        protected int CompileShaderSource(ShaderType t, string code)
        {
            int shaderId = Gl.CreateShader(t);
            Gl.ShaderSource(shaderId, code);
            Gl.CompileShader(shaderId);
            Gl.GetShader(shaderId, ShaderParameter.CompileStatus, out int success);
            if (success < 1)
            {
                string infoLog = null;
                Gl.GetShader(shaderId, ShaderParameter.InfoLogLength, out int loglen);
                if (loglen > 0)
                {
                    string log;
                    Gl.GetShaderInfoLog(shaderId, loglen, out _, out log);
                    infoLog = log;
                }
                throw new ShaderException(this, "Shader compilation failed. Type:" + t.ToString(), infoLog);
            }

            return shaderId;
        }

        protected int LinkShaderProgram(int vertexId, int fragmentId)
        {
            int programId = Gl.CreateProgram();
            Gl.AttachShader(programId, vertexId);
            Gl.AttachShader(programId, fragmentId);
            Gl.LinkProgram(programId);

            Gl.GetProgram(programId, GetProgramParameterName.LinkStatus, out int success);
            if (success < 1)
            {
                string infoLog = null;
                Gl.GetProgram(programId, GetProgramParameterName.InfoLogLength, out int loglen);
                if (loglen > 0)
                {
                    string log;
                    Gl.GetProgramInfoLog(programId, loglen, out _, out log);
                    infoLog = log;
                }
                throw new ShaderException(this, "Program Link failed.", infoLog);
            }

            //Gl.ValidateProgram(programId);
            //Gl.GetProgram(programId, ProgramProperty.ValidateStatus, out success);
            //if (success < 1)
            //{
            //    string infoLog = null;
            //    Gl.GetProgram(programId, ProgramProperty.InfoLogLength, out int loglen);
            //    if (loglen > 0)
            //    {
            //        StringBuilder log = new StringBuilder(loglen);
            //        Gl.GetProgramInfoLog(programId, loglen, out _, log);
            //        infoLog = log.ToString();
            //    }
            //    throw new ShaderException(this, "Program Validation failed.", infoLog);
            //}

            return programId;
        }

        protected void DestroyShader(int shaderId)
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
