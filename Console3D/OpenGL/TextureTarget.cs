#if EMBEDDED_GL
using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D.OpenGL
{
	// Requires GL3.2
    public enum TextureTarget
    {
		/// <summary>
		/// Strongly typed for value GL_PROXY_TEXTURE_1D, GL_PROXY_TEXTURE_1D_EXT 0x8063.
		/// </summary>
		ProxyTexture1d = 0x8063,

		/// <summary>
		/// Strongly typed for value GL_PROXY_TEXTURE_1D_ARRAY, GL_PROXY_TEXTURE_1D_ARRAY_EXT 0x8C19.
		/// </summary>
		ProxyTexture1dArray = 0x8C19,

		/// <summary>
		/// Strongly typed for value GL_PROXY_TEXTURE_2D, GL_PROXY_TEXTURE_2D_EXT 0x8064.
		/// </summary>
		ProxyTexture2d = 0x8064,

		/// <summary>
		/// Strongly typed for value GL_PROXY_TEXTURE_2D_ARRAY, GL_PROXY_TEXTURE_2D_ARRAY_EXT 0x8C1B.
		/// </summary>
		ProxyTexture2dArray = 0x8C1B,

		/// <summary>
		/// Strongly typed for value GL_PROXY_TEXTURE_2D_MULTISAMPLE 0x9101.
		/// </summary>
		ProxyTexture2dMultisample = 0x9101,

		/// <summary>
		/// Strongly typed for value GL_PROXY_TEXTURE_2D_MULTISAMPLE_ARRAY 0x9103.
		/// </summary>
		ProxyTexture2dMultisampleArray = 0x9103,

		/// <summary>
		/// Strongly typed for value GL_PROXY_TEXTURE_3D, GL_PROXY_TEXTURE_3D_EXT 0x8070.
		/// </summary>
		ProxyTexture3d = 0x8070,

		/// <summary>
		/// Strongly typed for value GL_PROXY_TEXTURE_CUBE_MAP, GL_PROXY_TEXTURE_CUBE_MAP_ARB, GL_PROXY_TEXTURE_CUBE_MAP_EXT 0x851B.
		/// </summary>
		ProxyTextureCubeMap = 0x851B,

		/// <summary>
		/// Strongly typed for value GL_PROXY_TEXTURE_RECTANGLE, GL_PROXY_TEXTURE_RECTANGLE_ARB, GL_PROXY_TEXTURE_RECTANGLE_NV 0x84F7.
		/// </summary>
		ProxyTextureRectangle = 0x84F7,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_1D 0x0DE0.
		/// </summary>
		Texture1d = 0x0DE0,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_2D 0x0DE1.
		/// </summary>
		Texture2d = 0x0DE1,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_3D, GL_TEXTURE_3D_EXT, GL_TEXTURE_3D_OES 0x806F.
		/// </summary>
		Texture3d = 0x806F,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_RECTANGLE 0x84F5.
		/// </summary>
		TextureRectangle = 0x84F5,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_CUBE_MAP 0x8513.
		/// </summary>
		TextureCubeMap = 0x8513,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_CUBE_MAP_POSITIVE_X 0x8515.
		/// </summary>
		TextureCubeMapPositiveX = 0x8515,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_CUBE_MAP_NEGATIVE_X 0x8516.
		/// </summary>
		TextureCubeMapNegativeX = 0x8516,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_CUBE_MAP_POSITIVE_Y 0x8517.
		/// </summary>
		TextureCubeMapPositiveY = 0x8517,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_CUBE_MAP_NEGATIVE_Y 0x8518.
		/// </summary>
		TextureCubeMapNegativeY = 0x8518,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_CUBE_MAP_POSITIVE_Z 0x8519.
		/// </summary>
		TextureCubeMapPositiveZ = 0x8519,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_CUBE_MAP_NEGATIVE_Z 0x851A.
		/// </summary>
		TextureCubeMapNegativeZ = 0x851A,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_1D_ARRAY 0x8C18.
		/// </summary>
		Texture1dArray = 0x8C18,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_2D_ARRAY 0x8C1A.
		/// </summary>
		Texture2dArray = 0x8C1A,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_2D_MULTISAMPLE 0x9100.
		/// </summary>
		Texture2dMultisample = 0x9100,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_2D_MULTISAMPLE_ARRAY 0x9102.
		/// </summary>
		Texture2dMultisampleArray = 0x9102,
	}
}
#endif