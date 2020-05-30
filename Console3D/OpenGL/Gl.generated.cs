#if EMBEDDED_GL
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Console3D.OpenGL
{
    public static partial class Gl
    {
        // Members
        /* API_GEN_MEMBERS */


        // Delegates
        /* API_GEN_DELEGATES */
        public delegate void glClearColorHandler(float r, float g, float b, float a);
        public delegate void glCleariHandler(int mask);
        public delegate int glGetErroriHandler();
        internal unsafe delegate void glGenBuffersHandler(int n, uint* buffers);
        public delegate void glActiveTextureHandler(int unit);
        public delegate void glBufferDataHandler(int target, uint size, IntPtr data, int usage);
        public delegate void glVertexAttribPointerHandler(uint index, int size, int type, [MarshalAs(UnmanagedType.I1)] bool normalized, int stride, IntPtr pointer);
        public delegate void glEnableVertexAttribArrayHandler(uint index);
        public delegate void glBindBufferHandler(int target, uint bufferid);
        public delegate void glBindVertexArrayHandler(uint id);
        internal unsafe delegate void glGenVertexArraysHandler(int n, uint* arrays);
        internal unsafe delegate void glGenTexturesHandler(int n, uint* textures);
        public delegate void glBindTextureHandler(int target, uint id);
        public delegate void glTexParameterpHandler(int target, int pname, IntPtr param);
        internal unsafe delegate void glTexParameterfvHandler(int target, int pname, void* param);
        public delegate void glDrawElementsiHandler(int mode, int count, int type, IntPtr indices);
        public delegate int glGetUniformLocationHandler(uint program, string name);
        internal unsafe delegate void glUniform1fHandler(int program, float* value);
        internal unsafe delegate void glUniform1iHandler(int program, int* value);
        internal unsafe delegate void glUniform1dHandler(int program, double* value);
        internal unsafe delegate void glUniform1uiHandler(int program, uint* value);
        public delegate void glEnableiHandler(int cap);
        public delegate void glDisableiHandler(int cap);
        internal unsafe delegate void glUniform2fHandler(int program, float* v1, float* v2);
        internal unsafe delegate void glUniform2iHandler(int program, int* v1, int* v2);
        internal unsafe delegate void glUniform2dHandler(int program, double* v1, double* v2);
        internal unsafe delegate void glUniform2uiHandler(int program, uint* v1, uint* v2);
        internal unsafe delegate void glUniform3fHandler(int program, float* v1, float* v2, float* v3);
        internal unsafe delegate void glUniform3iHandler(int program, int* v1, int* v2, int* v3);
        internal unsafe delegate void glUniform3dHandler(int program, double* v1, double* v2, double* v3);
        internal unsafe delegate void glUniform3uiHandler(int program, uint* v1, uint* v2, uint* v3);
        internal unsafe delegate void glUniform4fHandler(int program, float* v1, float* v2, float* v3, float* v4);
        internal unsafe delegate void glUniform4iHandler(int program, int* v1, int* v2, int* v3, int* v4);
        internal unsafe delegate void glUniform4dHandler(int program, double* v1, double* v2, double* v3, double* v4);
        internal unsafe delegate void glUniform4uiHandler(int program, uint* v1, uint* v2, uint* v3, uint* v4);
        public delegate void glUseProgramHandler(uint program);
        public delegate uint glCreateShaderiHandler(int type);
        internal unsafe delegate void glShaderSourcepHandler(uint shader, int count, string[] code, int* length);
        public delegate uint glDeleteShaderHandler(uint id);
        public delegate uint glDeleteProgramHandler(uint id);
        public delegate uint glAttachShaderHandler(uint program, uint shader);
        public delegate void glLinkProgramHandler(uint program);
        public delegate void glCompileShaderHandler(uint shader);
        public delegate uint glCreateProgramHandler();
        internal unsafe delegate void glGetShaderInfoLogpHandler(uint shader, int bufSize, int* length, StringBuilder infoLog);
        internal unsafe delegate void glGetProgramInfoLogpHandler(uint program, int bufSize, int* length, StringBuilder infoLog);
        internal unsafe delegate void glGetShaderiv(uint shader, int pname, int* values);
        internal unsafe delegate void glGetProgramiv(uint program, int pname, int* values);
        public delegate void glViewportHandler(int x, int y, int width, int height);
        public delegate IntPtr glGetStringHandler(int name);
        public delegate void glEndHandler();
        public delegate void glBeginiHandler(int mode);
        public delegate void glColor3fHandler(float r, float g, float b);
        public delegate void glVertex2fHandler(float x, float y);






        // Methods
        /* API_GEN_METHODS */
        public static glClearColorHandler ClearColor;
        public static glCleariHandler Cleari;
        public static glGetErroriHandler GetErrori;
        internal static glGenBuffersHandler GenBuffers;
        public static glActiveTextureHandler ActiveTexturei;
        public static glBufferDataHandler BufferDatap;
        public static glVertexAttribPointerHandler VertexAttribPointerp;
        public static glEnableVertexAttribArrayHandler EnableVertexAttribArray;
        public static glBindBufferHandler BindBufferi;
        public static glBindVertexArrayHandler BindVertexArray;
        internal static glGenVertexArraysHandler GenVertexArrays;
        internal static glGenTexturesHandler GenTextures;
        public static glBindTextureHandler BindTexturei;
        public static glTexParameterpHandler TexParameterp;
        internal static glTexParameterfvHandler TexParameterfv;
        public static glDrawElementsiHandler DrawElementsi;
        public static glGetUniformLocationHandler GetUniformLocation;
        internal static glUniform1uiHandler Uniform1ui;
        internal static glUniform1iHandler Uniform1i;
        internal static glUniform1dHandler Uniform1d;
        internal static glUniform1fHandler Uniform1f;
        public static glEnableiHandler Enablei;
        public static glDisableiHandler Disablei;
        internal static glUniform2uiHandler Uniform2ui;
        internal static glUniform2iHandler Uniform2i;
        internal static glUniform2dHandler Uniform2d;
        internal static glUniform2fHandler Uniform2f;
        internal static glUniform3uiHandler Uniform3ui;
        internal static glUniform3iHandler Uniform3i;
        internal static glUniform3dHandler Uniform3d;
        internal static glUniform3fHandler Uniform3f;
        internal static glUniform4uiHandler Uniform4ui;
        internal static glUniform4iHandler Uniform4i;
        internal static glUniform4dHandler Uniform4d;
        internal static glUniform4fHandler Uniform4f;
        public static glUseProgramHandler UseProgram;
        public static glCreateShaderiHandler CreateShaderi;
        internal static glShaderSourcepHandler ShaderSourcep;
        public static glDeleteProgramHandler DeleteProgram;
        public static glDeleteShaderHandler DeleteShader;
        public static glAttachShaderHandler AttachShader;
        public static glLinkProgramHandler LinkProgram;
        public static glCompileShaderHandler CompileShader;
        public static glCreateProgramHandler CreateProgram;
        internal static glGetShaderInfoLogpHandler GetShaderInfoLogp;
        internal static glGetProgramInfoLogpHandler GetProgramInfoLogp;
        internal static glGetShaderiv GetShaderiv;
        internal static glGetProgramiv GetProgramiv;
        public static glViewportHandler Viewport;
        public static glGetStringHandler GetStringpi;
        public static glEndHandler End;
        public static glBeginiHandler Begini;
        public static glColor3fHandler Color3f;
        public static glVertex2fHandler Vertex2f;
    }
}
#endif