using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D.OpenGL
{
	// Requires GL3.2
    public enum PrimitiveType
    {
		/// <summary>
		/// Strongly typed for value GL_LINES. 0x0001
		/// </summary>
		Lines = 0x0001,

		/// <summary>
		/// Strongly typed for value GL_LINES_ADJACENCY, GL_LINES_ADJACENCY_ARB, GL_LINES_ADJACENCY_EXT. 0x000A
		/// </summary>
		LinesAdjacency = 0x000A,

		/// <summary>
		/// Strongly typed for value GL_LINE_LOOP. 0x0002
		/// </summary>
		LineLoop = 0x0002,

		/// <summary>
		/// Strongly typed for value GL_LINE_STRIP. 0x0003
		/// </summary>
		LineStrip = 0x0003,

		/// <summary>
		/// Strongly typed for value GL_LINE_STRIP_ADJACENCY, GL_LINE_STRIP_ADJACENCY_ARB, GL_LINE_STRIP_ADJACENCY_EXT. 0x000B
		/// </summary>
		LineStripAdjacency = 0x000B,

		/// <summary>
		/// Strongly typed for value GL_POINTS. 0x0000
		/// </summary>
		Points = 0x0000,

		/// <summary>
		/// Strongly typed for value GL_POLYGON. 0x0009
		/// </summary>
		Polygon = 0x0009,

		/// <summary>
		/// Strongly typed for value GL_QUADS, GL_QUADS_EXT. 0x0007
		/// </summary>
		Quads = 0x0007,

		/// <summary>
		/// Strongly typed for value GL_QUAD_STRIP. 0x0008
		/// </summary>
		QuadStrip = 0x0008,

		/// <summary>
		/// Strongly typed for value GL_TRIANGLES. 0x0004
		/// </summary>
		Triangles = 0x0004,

		/// <summary>
		/// Strongly typed for value GL_TRIANGLES_ADJACENCY, GL_TRIANGLES_ADJACENCY_ARB, GL_TRIANGLES_ADJACENCY_EXT. 0x000C
		/// </summary>
		TrianglesAdjacency = 0x000C,

		/// <summary>
		/// Strongly typed for value GL_TRIANGLE_FAN. 0x0006
		/// </summary>
		TriangleFan = 0x0006,

		/// <summary>
		/// Strongly typed for value GL_TRIANGLE_STRIP. 0x0005
		/// </summary>
		TriangleStrip = 0x0005,

		/// <summary>
		/// Strongly typed for value GL_TRIANGLE_STRIP_ADJACENCY, GL_TRIANGLE_STRIP_ADJACENCY_ARB, GL_TRIANGLE_STRIP_ADJACENCY_EXT. 0x000D
		/// </summary>
		TriangleStripAdjacency = 0x000D,

	}
}
 