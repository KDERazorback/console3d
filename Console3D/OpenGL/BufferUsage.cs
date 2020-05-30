#if EMBEDDED_GL
using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D.OpenGL
{
	// Required GL1.5
    public enum BufferUsage
    {
		/// <summary>
		/// Strongly typed for value GL_STREAM_DRAW. 0x88E0
		/// </summary>
		StreamDraw = 0x88E0,

		/// <summary>
		/// Strongly typed for value GL_STREAM_READ. 0x88E1
		/// </summary>
		StreamRead = 0x88E1,

		/// <summary>
		/// Strongly typed for value GL_STREAM_COPY. 0x88E2
		/// </summary>
		StreamCopy = 0x88E2,

		/// <summary>
		/// Strongly typed for value GL_STATIC_DRAW. 0x88E4
		/// </summary>
		StaticDraw = 0x88E4,

		/// <summary>
		/// Strongly typed for value GL_STATIC_READ. 0x88E5
		/// </summary>
		StaticRead = 0x88E5,

		/// <summary>
		/// Strongly typed for value GL_STATIC_COPY. 0x88E6
		/// </summary>
		StaticCopy = 0x88E6,

		/// <summary>
		/// Strongly typed for value GL_DYNAMIC_DRAW. 0x88E8
		/// </summary>
		DynamicDraw = 0x88E8,

		/// <summary>
		/// Strongly typed for value GL_DYNAMIC_READ. 0x88E9
		/// </summary>
		DynamicRead = 0x88E9,

		/// <summary>
		/// Strongly typed for value GL_DYNAMIC_COPY. 0x88EA
		/// </summary>
		DynamicCopy = 0x88EA,

	}
}
#endif