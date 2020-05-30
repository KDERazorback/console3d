using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D.OpenGL
{
	// Requires GL1.4
    public enum TextureParameterName
    {
		/// <summary>
		/// Strongly typed for value GL_GENERATE_MIPMAP, GL_GENERATE_MIPMAP_SGIS. 0x8191
		/// </summary>
		GenerateMipmap = 0x8191,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_BORDER_COLOR, GL_TEXTURE_BORDER_COLOR_NV. 0x1004
		/// </summary>
		TextureBorderColor = 0x1004,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_MAG_FILTER. 0x2800
		/// </summary>
		TextureMagFilter = 0x2800,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_MIN_FILTER. 0x2801
		/// </summary>
		TextureMinFilter = 0x2801,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_PRIORITY, GL_TEXTURE_PRIORITY_EXT. 0x8066
		/// </summary>
		TexturePriority = 0x8066,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_WRAP_R, GL_TEXTURE_WRAP_R_EXT, GL_TEXTURE_WRAP_R_OES. 0x8072
		/// </summary>
		TextureWrapR = 0x8072,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_WRAP_S. 0x2802
		/// </summary>
		TextureWrapS = 0x2802,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_WRAP_T. 0x2803
		/// </summary>
		TextureWrapT = 0x2803,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_BASE_LEVEL, GL_TEXTURE_BASE_LEVEL_SGIS. 0x813C
		/// </summary>
		TextureBaseLevel = 0x813C,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_COMPARE_MODE. 0x884C
		/// </summary>
		TextureCompareMode = 0x884C,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_COMPARE_FUNC. 0x884D
		/// </summary>
		TextureCompareFunc = 0x884D,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_LOD_BIAS. 0x8501
		/// </summary>
		TextureLodBias = 0x8501,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_MIN_LOD, GL_TEXTURE_MIN_LOD_SGIS. 0x813A
		/// </summary>
		TextureMinLod = 0x813A,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_MAX_LOD, GL_TEXTURE_MAX_LOD_SGIS. 0x813B
		/// </summary>
		TextureMaxLod = 0x813B,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_MAX_LEVEL, GL_TEXTURE_MAX_LEVEL_SGIS. 0x813D
		/// </summary>
		TextureMaxLevel = 0x813D,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_ALPHA_SIZE. 0x805F
		/// </summary>
		TextureAlphaSize = 0x805F,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_BLUE_SIZE. 0x805E
		/// </summary>
		TextureBlueSize = 0x805E,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_BORDER. 0x1005
		/// </summary>
		TextureBorder = 0x1005,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_COMPONENTS, GL_TEXTURE_INTERNAL_FORMAT. 0x1003
		/// </summary>
		TextureComponents = 0x1003,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_COMPONENTS, GL_TEXTURE_INTERNAL_FORMAT. 0x1003
		/// </summary>
		TextureInternalFormat = 0x1003,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_DEPTH_EXT. 0x8071
		/// </summary>
		TextureDepth = 0x8071,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_GREEN_SIZE. 0x805D
		/// </summary>
		TextureGreenSize = 0x805D,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_HEIGHT. 0x1001
		/// </summary>
		TextureHeight = 0x1001,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_INTENSITY_SIZE. 0x8061
		/// </summary>
		TextureIntensitySize = 0x8061,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_LUMINANCE_SIZE. 0x8060
		/// </summary>
		TextureLuminanceSize = 0x8060,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_RED_SIZE. 0x805C
		/// </summary>
		TextureRedSize = 0x805C,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_RESIDENT. 0x8067
		/// </summary>
		TextureResident = 0x8067,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_WIDTH. 0x1000
		/// </summary>
		TextureWidth = 0x1000,

    }
}
