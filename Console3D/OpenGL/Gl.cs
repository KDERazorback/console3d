#if EMBEDDED_GL
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Console3D.OpenGL
{
    public static partial class Gl
    {
        public static bool IsApiBound { get; private set; }
        public static void BindApi()
        {
            _Internal_BindApi();

            IsApiBound = true;
        }

        public static void UnbindApi()
        {
            IsApiBound = false;

            _Internal_UnbindApi();
        }

        public static void Clear(ClearBufferMask mask)
        {
            Cleari((int)mask);
        }

        public static uint GenBuffer()
        {
            uint addr;
            unsafe { GenBuffers(1, &addr); }

            return addr;
        }

        public static void ActiveTexture(TextureUnit unit)
        {
            ActiveTexturei((int)unit);
        }

        public static void BufferData(BufferTarget target, uint size, object data, BufferUsage usage)
        {
            GCHandle pin_data = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                BufferDatap((int)target, size, pin_data.AddrOfPinnedObject(), (int)usage);
            }
            finally
            {
                pin_data.Free();
            }
        }

        public static void VertexAttribPointer(uint index, int size, VertexAttribType type, bool normalized, int stride, object pointer)
        {
            GCHandle pin_pointer = GCHandle.Alloc(pointer, GCHandleType.Pinned);
            try
            {
                VertexAttribPointerp(index, size, (int)type, normalized, stride, pin_pointer.AddrOfPinnedObject());
            }
            finally
            {
                pin_pointer.Free();
            }
        }

        public static void BindBuffer(BufferTarget target, uint bufferid)
        {
            BindBufferi((int)target, bufferid);
        }

        public static uint GenVertexArray()
        {
            uint addr;
            unsafe { GenVertexArrays(1, &addr); }

            return addr;
        }

        public static uint GenTexture()
        {
            uint addr;
            unsafe { GenTextures(1, &addr); }

            return addr;
        }

        public static void BindTexture(TextureTarget target, uint id)
        {
            BindTexturei((int)target, id);
        }

        public static void TexParameter(TextureTarget target, TextureParameterName name, float[] data)
        {
            unsafe
            {
                fixed (float* p_params = data)
                    TexParameterfv((int)target, (int)name, p_params);
            }
        }

        public static void DrawElements(PrimitiveType mode, int count, DrawElementsType type, object indices)
        {
            GCHandle pin_indices = GCHandle.Alloc(indices, GCHandleType.Pinned);
            try
            {
                DrawElementsi((int)mode, count, (int)type, pin_indices.AddrOfPinnedObject());
            }
            finally
            {
                pin_indices.Free();
            }
        }

        public static void Uniform1(int location, float value)
        {
            unsafe
            {
                Uniform1f(location, &value);
            }
        }

        public static void Uniform1(int location, double value)
        {
            unsafe
            {
                Uniform1d(location, &value);
            }
        }

        public static void Uniform1(int location, int value)
        {
            unsafe
            {
                Uniform1i(location, &value);
            }
        }

        public static void Uniform1(int location, uint value)
        {
            unsafe
            {
                Uniform1ui(location, &value);
            }
        }

        public static void Uniform2(int location, uint v1, uint v2)
        {
            unsafe
            {
                Uniform2ui(location, &v1, &v2);
            }
        }

        public static void Uniform2(int location, int v1, int v2)
        {
            unsafe
            {
                Uniform2i(location, &v1, &v2);
            }
        }

        public static void Uniform2(int location, float v1, float v2)
        {
            unsafe
            {
                Uniform2f(location, &v1, &v2);
            }
        }

        public static void Uniform2(int location, double v1, double v2)
        {
            unsafe
            {
                Uniform2d(location, &v1, &v2);
            }
        }

        public static void Uniform3(int location, uint v1, uint v2, uint v3)
        {
            unsafe
            {
                Uniform3ui(location, &v1, &v2, &v3);
            }
        }

        public static void Uniform3(int location, int v1, int v2, int v3)
        {
            unsafe
            {
                Uniform3i(location, &v1, &v2, &v3);
            }
        }

        public static void Uniform3(int location, float v1, float v2, float v3)
        {
            unsafe
            {
                Uniform3f(location, &v1, &v2, &v3);
            }
        }

        public static void Uniform3(int location, double v1, double v2, double v3)
        {
            unsafe
            {
                Uniform3d(location, &v1, &v2, &v3);
            }
        }

        public static void Uniform4(int location, uint v1, uint v2, uint v3, uint v4)
        {
            unsafe
            {
                Uniform4ui(location, &v1, &v2, &v3, &v4);
            }
        }

        public static void Uniform4(int location, int v1, int v2, int v3, int v4)
        {
            unsafe
            {
                Uniform4i(location, &v1, &v2, &v3, &v4);
            }
        }

        public static void Uniform4(int location, float v1, float v2, float v3, float v4)
        {
            unsafe
            {
                Uniform4f(location, &v1, &v2, &v3, &v4);
            }
        }

        public static void Uniform4(int location, double v1, double v2, double v3, double v4)
        {
            unsafe
            {
                Uniform4d(location, &v1, &v2, &v3, &v4);
            }
        }

        public static void Enable(EnableCap cap)
        {
            Enablei((int)cap);
        }

        public static void Disable(EnableCap cap)
        {
            Disablei((int)cap);
        }

        public static uint CreateShader(ShaderType type)
        {
            return CreateShaderi((int)type);
        }

        public static void ShaderSource(uint shaderId, string[] code)
        {
            int[] lens = new int[code.Length];

            for (int i = 0; i < lens.Length; i++)
                lens[i] = code[i].Length;

            unsafe
            {
                fixed (int* p_length = lens)
                    ShaderSourcep(shaderId, lens.Length, code, p_length);
            }
        }

        public static void ShaderSource(uint shaderId, string code)
        {
            ShaderSource(shaderId, new string[] { code });
        }

        public static void GetShaderInfoLog(uint shader, int maxLength, out int length, StringBuilder infoLog)
        {
            unsafe
            {
                fixed (int* p_length = &length)
                    GetShaderInfoLogp(shader, maxLength, p_length, infoLog);
            }
        }

        public static void GetProgramInfoLog(uint program, int maxLength, out int length, StringBuilder infoLog)
        {
            unsafe
            {
                fixed (int* p_length = &length)
                    GetProgramInfoLogp(program, maxLength, p_length, infoLog);
            }
        }

        public static void GetShader(uint shader, ShaderParameterName name, out int values)
        {
            unsafe
            {
                fixed (int* p_params = &values)
                    GetShaderiv(shader, (int)name, p_params);
            }
        }

        public static void GetProgram(uint program, ProgramProperty name, out int values)
        {
            unsafe
            {
                fixed (int* p_params = &values)
                    GetProgramiv(program, (int)name, p_params);
            }
        }

        public static string GetString(StringName name)
        {
            IntPtr val = GetStringpi((int)name);

            if (val == IntPtr.Zero)
                return null;

            List<byte> buff = new List<byte>();
            int offset = 0;

            while (true)
            {
                byte currentByte = Marshal.ReadByte(val, offset);
                if (currentByte == 0)
                    break;
                buff.Add(currentByte);
                offset++;
            }

            return Encoding.UTF8.GetString(buff.ToArray());
        }

        public static KhronosVersion CurrentVersion
        {
            get
            {
                return KhronosVersion.Parse(GetString(StringName.Version), null);
            }
        }

        public static string CurrentVendor => GetString(StringName.Vendor);
        public static string CurrentRenderer => GetString(StringName.Renderer);

        public static ErrorCode GetError()
        {
            return (ErrorCode)GetErrori();
        }

        public static void Initialize()
        {
            if (IsApiBound)
                return;

            BindApi();
        }

        public static void Begin(PrimitiveType mode)
        {
            Begini((int)mode);
        }

        public static void Color3(float r, float g, float b)
        {
            Color3f(r, g, b);
        }

        public static void Vertex2(float x, float y)
        {
            Vertex2f(x, y);
        }
    }
}
#endif