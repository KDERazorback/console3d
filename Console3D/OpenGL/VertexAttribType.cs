#if EMBEDDED_GL
using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D.OpenGL
{
	// Requires GL3.0
    public enum VertexAttribType
    {
		/// <summary>
		/// Strongly typed for value GL_BYTE. 0x1400
		/// </summary>
		Byte = Gl.GL_BYTE,

		/// <summary>
		/// Strongly typed for value GL_SHORT. 0x1402
		/// </summary>
		Short = Gl.GL_SHORT,

		/// <summary>
		/// Strongly typed for value GL_INT. 0x1404
		/// </summary>
		Int = Gl.GL_INT,

		/// <summary>
		/// Strongly typed for value GL_FLOAT. 0x1406
		/// </summary>
		Float = Gl.GL_FLOAT,

		/// <summary>
		/// Strongly typed for value GL_HALF_FLOAT.
		/// </summary>
		HalfFloat = Gl.GL_HALF_FLOAT,

		/// <summary>
		/// Strongly typed for value GL_DOUBLE.
		/// </summary>
		Double = Gl.GL_DOUBLE,

		/// <summary>
		/// Strongly typed for value GL_UNSIGNED_BYTE.
		/// </summary>
		UnsignedByte = Gl.GL_UNSIGNED_BYTE,

		/// <summary>
		/// Strongly typed for value GL_UNSIGNED_SHORT.
		/// </summary>
		UnsignedShort = Gl.GL_UNSIGNED_SHORT,

		/// <summary>
		/// Strongly typed for value GL_UNSIGNED_INT.
		/// </summary>
		UnsignedInt = Gl.GL_UNSIGNED_INT,

		/// <summary>
		/// Strongly typed for value GL_UNSIGNED_INT_2_10_10_10_REV. 0x8368
		/// </summary>
		UnsignedInt2101010Rev = 0x8368,

		/// <summary>
		/// Strongly typed for value GL_UNSIGNED_INT_10F_11F_11F_REV. 0x8C3B
		/// </summary>
		UnsignedInt10f11f11fRev = 0x8C3B,

	}
}
#endif