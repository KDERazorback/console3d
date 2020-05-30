#if EMBEDDED_GL
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Console3D.OpenGL
{
    public static partial class Gl
    {
        // Binding targets
        private static readonly KeyValuePair<string, string>[] _Internal_DelegateTargets = new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>(nameof(ClearColor), "glClearColor"),
            new KeyValuePair<string, string>(nameof(Cleari), "glClear"),
            new KeyValuePair<string, string>(nameof(GetErrori), "glGetError"),
            new KeyValuePair<string, string>(nameof(GenBuffers), "glGenBuffers"),
            new KeyValuePair<string, string>(nameof(ActiveTexturei), "glActiveTexture"),
            new KeyValuePair<string, string>(nameof(BufferDatap), "glBufferData"),
            new KeyValuePair<string, string>(nameof(VertexAttribPointerp), "glVertexAttribPointer"),
            new KeyValuePair<string, string>(nameof(EnableVertexAttribArray), "glEnableVertexAttribArray"),
            new KeyValuePair<string, string>(nameof(BindBufferi), "glBindBuffer"),
            new KeyValuePair<string, string>(nameof(BindVertexArray), "glBindVertexArray"),
            new KeyValuePair<string, string>(nameof(GenVertexArrays), "glGenVertexArrays"),
            new KeyValuePair<string, string>(nameof(GenTextures), "glGenTextures"),
            new KeyValuePair<string, string>(nameof(BindTexturei), "glBindTexture"),
            new KeyValuePair<string, string>(nameof(TexParameterp), "glTexParameterx"),
            new KeyValuePair<string, string>(nameof(TexParameterfv), "glTexParameterfv"),
            new KeyValuePair<string, string>(nameof(DrawElementsi), "glDrawElements"),
            new KeyValuePair<string, string>(nameof(GetUniformLocation), "glGetUniformLocation"),
            new KeyValuePair<string, string>(nameof(Uniform1ui), "glUniform1ui"),
            new KeyValuePair<string, string>(nameof(Uniform1i), "glUniform1i"),
            new KeyValuePair<string, string>(nameof(Uniform1d), "glUniform1d"),
            new KeyValuePair<string, string>(nameof(Uniform1f), "glUniform1f"),
            new KeyValuePair<string, string>(nameof(Enablei), "glEnable"),
            new KeyValuePair<string, string>(nameof(Disablei), "glDisable"),
            new KeyValuePair<string, string>(nameof(Uniform2ui), "glUniform2ui"),
            new KeyValuePair<string, string>(nameof(Uniform2i), "glUniform2i"),
            new KeyValuePair<string, string>(nameof(Uniform2d), "glUniform2d"),
            new KeyValuePair<string, string>(nameof(Uniform2f), "glUniform2f"),
            new KeyValuePair<string, string>(nameof(Uniform3ui), "glUniform3ui"),
            new KeyValuePair<string, string>(nameof(Uniform3i), "glUniform3i"),
            new KeyValuePair<string, string>(nameof(Uniform3d), "glUniform3d"),
            new KeyValuePair<string, string>(nameof(Uniform3f), "glUniform3f"),
            new KeyValuePair<string, string>(nameof(Uniform4ui), "glUniform4ui"),
            new KeyValuePair<string, string>(nameof(Uniform4i), "glUniform4i"),
            new KeyValuePair<string, string>(nameof(Uniform4d), "glUniform4d"),
            new KeyValuePair<string, string>(nameof(Uniform4f), "glUniform4f"),
            new KeyValuePair<string, string>(nameof(UseProgram), "glUseProgram"),
            new KeyValuePair<string, string>(nameof(CreateShaderi), "glCreateShader"),
            new KeyValuePair<string, string>(nameof(ShaderSourcep), "glShaderSource"),
            new KeyValuePair<string, string>(nameof(DeleteProgram), "glDeleteProgram"),
            new KeyValuePair<string, string>(nameof(DeleteShader), "glDeleteShader"),
            new KeyValuePair<string, string>(nameof(AttachShader), "glAttachShader"),
            new KeyValuePair<string, string>(nameof(LinkProgram), "glLinkProgram"),
            new KeyValuePair<string, string>(nameof(CompileShader), "glCompileShader"),
            new KeyValuePair<string, string>(nameof(CreateProgram), "glCreateProgram"),
            new KeyValuePair<string, string>(nameof(GetShaderInfoLogp), "glGetShaderInfoLog"),
            new KeyValuePair<string, string>(nameof(GetProgramInfoLogp), "glGetProgramInfoLog"),
            new KeyValuePair<string, string>(nameof(GetShaderiv), "glGetShaderiv"),
            new KeyValuePair<string, string>(nameof(GetProgramiv), "glGetProgramiv"),
            new KeyValuePair<string, string>(nameof(Viewport), "glViewport"),
            new KeyValuePair<string, string>(nameof(GetStringpi), "glGetString"),
            new KeyValuePair<string, string>(nameof(End), "glEnd"),
            new KeyValuePair<string, string>(nameof(Begini), "glBegin"),
            new KeyValuePair<string, string>(nameof(Color3f), "glColor3f"),
            new KeyValuePair<string, string>(nameof(Vertex2f), "glVertex2f"),

        };

        // Binding
        private static void _Internal_BindApi()
        {
            /* API_GEN_BINDER */
            Type thisType = typeof(Gl);
            foreach (KeyValuePair<string, string> entry in _Internal_DelegateTargets)
            {
                try
                {
                    FieldInfo field = thisType.GetField(entry.Key, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                    IntPtr address = GLFW.Glfw.GetProcAddress(entry.Value);

                    if (address == IntPtr.Zero)
                        throw new InvalidCastException("Cannot locate entrypoint for unmanaged function " + entry.Value);

                    field.SetValue(null, Marshal.GetDelegateForFunctionPointer(address, field.FieldType));
                }
                catch (Exception ex)
                {
                    
                }
            }

            //ClearColor = Marshal.GetDelegateForFunctionPointer<glClearColorHandler>(GLFW.Glfw.GetProcAddress("glClearColor"));
            //Clear = Marshal.GetDelegateForFunctionPointer<glClearHandler>(GLFW.Glfw.GetProcAddress("glClear"));
        }

        // Unbinding
        private static void _Internal_UnbindApi()
        {
            /* API_GEN_UNBINDER */
            ClearColor = null;
            Cleari = null;
            GetErrori = null;
            GenBuffers = null;
            ActiveTexturei = null;
            BufferDatap = null;
            VertexAttribPointerp = null;
            EnableVertexAttribArray = null;
            BindBufferi = null;
            BindVertexArray = null;
            GenVertexArrays = null;
            GenTextures = null;
            BindTexturei = null;
            TexParameterp = null;
            TexParameterfv = null;
            DrawElementsi = null;
            GetUniformLocation = null;
            Uniform1ui = null;
            Uniform1i = null;
            Uniform1d = null;
            Uniform1f = null;
            Enablei = null;
            Disablei = null;
            Uniform2ui = null;
            Uniform2i = null;
            Uniform2d = null;
            Uniform2f = null;
            Uniform3ui = null;
            Uniform3i = null;
            Uniform3d = null;
            Uniform3f = null;
            Uniform4ui = null;
            Uniform4i = null;
            Uniform4d = null;
            Uniform4f = null;
            UseProgram = null;
            CreateShaderi = null;
            ShaderSourcep = null;
            DeleteProgram = null;
            DeleteShader = null;
            AttachShader = null;
            LinkProgram = null;
            CompileShader = null;
            CreateProgram = null;
            GetShaderInfoLogp = null;
            GetProgramInfoLogp = null;
            GetShaderiv = null;
            GetProgramiv = null;
            Viewport = null;
            GetStringpi = null;
            End = null;
            Begini = null;
            Color3f = null;
            Vertex2f = null;
        }
    }
}
#endif