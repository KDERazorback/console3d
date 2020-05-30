using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D.OpenGL
{
	// Requires Minimum GL1.3
    public enum TextureWrapMode
    {
		/// <summary>
		/// Strongly typed for value GL_CLAMP 0x2900.
		/// </summary>
		Clamp = 0x2900,

		/// <summary>
		/// Strongly typed for value GL_CLAMP_TO_BORDER 0x812D.
		/// </summary>
		ClampToBorder = 0x812D,

		/// <summary>
		/// Strongly typed for value GL_CLAMP_TO_EDGE 0x812F.
		/// </summary>
		ClampToEdge = 0x812F,

		/// <summary>
		/// Strongly typed for value GL_REPEAT 0x2901.
		/// </summary>
		Repeat = 0x2901,

    }
}
