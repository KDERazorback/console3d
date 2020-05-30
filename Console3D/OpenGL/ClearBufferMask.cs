#if EMBEDDED_GL
using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D.OpenGL
{
	// Requires GL1.0
    public enum ClearBufferMask : uint
    {
		/// <summary>
		/// Strongly typed for value GL_ACCUM_BUFFER_BIT. 0x00000200
		/// </summary>
		AccumBufferBit = 0x00000200,

		/// <summary>
		/// Strongly typed for value GL_COLOR_BUFFER_BIT. 0x00004000
		/// </summary>
		ColorBufferBit = 0x00004000,

		/// <summary>
		/// Strongly typed for value GL_DEPTH_BUFFER_BIT. 0x00000100
		/// </summary>
		DepthBufferBit = 0x00000100,

		/// <summary>
		/// Strongly typed for value GL_STENCIL_BUFFER_BIT. 0x00000400
		/// </summary>
		StencilBufferBit = 0x00000400,

	}
}
#endif