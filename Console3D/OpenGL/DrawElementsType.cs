using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D.OpenGL
{
	// Requires GL1.1
    public enum DrawElementsType
    {
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

	}
}
