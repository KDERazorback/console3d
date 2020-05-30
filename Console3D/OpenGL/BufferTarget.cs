#if EMBEDDED_GL
using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D.OpenGL
{
	// Requires GL3.1
    public enum BufferTarget
    {
		/// <summary>
		/// Strongly typed for value GL_ARRAY_BUFFER. 0x8892
		/// </summary>
		ArrayBuffer = 0x8892,

		/// <summary>
		/// Strongly typed for value GL_COPY_READ_BUFFER. 0x8F36
		/// </summary>
		CopyReadBuffer = 0x8F36,

		/// <summary>
		/// Strongly typed for value GL_COPY_WRITE_BUFFER. 0x8F37
		/// </summary>
		CopyWriteBuffer = 0x8F37,

		/// <summary>
		/// Strongly typed for value GL_ELEMENT_ARRAY_BUFFER. 0x8893
		/// </summary>
		ElementArrayBuffer = 0x8893,

		/// <summary>
		/// Strongly typed for value GL_PIXEL_PACK_BUFFER. 0x88EB
		/// </summary>
		PixelPackBuffer = 0x88EB,

		/// <summary>
		/// Strongly typed for value GL_PIXEL_UNPACK_BUFFER. 0x88EC
		/// </summary>
		PixelUnpackBuffer = 0x88EC,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_BUFFER. 0x8C2A
		/// </summary>
		TextureBuffer = 0x8C2A,

		/// <summary>
		/// Strongly typed for value GL_TRANSFORM_FEEDBACK_BUFFER. 0x8C8E
		/// </summary>
		TransformFeedbackBuffer = 0x8C8E,

		/// <summary>
		/// Strongly typed for value GL_UNIFORM_BUFFER. 0x8A11
		/// </summary>
		UniformBuffer = 0x8A11,

	}
}
#endif