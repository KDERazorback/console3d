#if EMBEDDED_GL
using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D.OpenGL
{
	// Requires GL2.0
    public enum ShaderParameterName
    {
		/// <summary>
		/// Strongly typed for value GL_SHADER_TYPE. 0x8B4F
		/// </summary>
		ShaderType = 0x8B4F,

		/// <summary>
		/// Strongly typed for value GL_DELETE_STATUS. 0x8B80
		/// </summary>
		DeleteStatus = 0x8B80,

		/// <summary>
		/// Strongly typed for value GL_COMPILE_STATUS. 0x8B81
		/// </summary>
		CompileStatus = 0x8B81,

		/// <summary>
		/// Strongly typed for value GL_INFO_LOG_LENGTH. 0x8B84
		/// </summary>
		InfoLogLength = 0x8B84,

		/// <summary>
		/// Strongly typed for value GL_SHADER_SOURCE_LENGTH. 0x8B88
		/// </summary>
		ShaderSourceLength = 0x8B88,

	}
}
#endif