#if EMBEDDED_GL
using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D.OpenGL
{
	// Requires GL3.2
    public enum ShaderType
    {
		/// <summary>
		/// Strongly typed for value GL_VERTEX_SHADER, GL_VERTEX_SHADER_ARB. 0x8B31
		/// </summary>
		VertexShader = 0x8B31,

		/// <summary>
		/// Strongly typed for value GL_GEOMETRY_SHADER. 0x8DD9
		/// </summary>
		GeometryShader = 0x8DD9,

		/// <summary>
		/// Strongly typed for value GL_FRAGMENT_SHADER, GL_FRAGMENT_SHADER_ARB. 0x8B30
		/// </summary>
		FragmentShader = 0x8B30,

    }
}
#endif