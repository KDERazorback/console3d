#if EMBEDDED_GL
using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D.OpenGL
{
	// Requires GL2.0
    public enum StringName
    {
		/// <summary>
		/// Strongly typed for value GL_EXTENSIONS. 0x1F03
		/// </summary>
		Extensions = 0x1F03,

		/// <summary>
		/// Strongly typed for value GL_RENDERER. 0x1F01
		/// </summary>
		Renderer = 0x1F01,

		/// <summary>
		/// Strongly typed for value GL_VENDOR. 0x1F00
		/// </summary>
		Vendor = 0x1F00,

		/// <summary>
		/// Strongly typed for value GL_VERSION. 0x1F02
		/// </summary>
		Version = 0x1F02,

		/// <summary>
		/// Strongly typed for value GL_SHADING_LANGUAGE_VERSION. 0x8B8C
		/// </summary>
		ShadingLanguageVersion = 0x8B8C,

	}
}
#endif
