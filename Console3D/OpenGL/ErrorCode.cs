using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D.OpenGL
{
	// Required GL3.0
    public enum ErrorCode
    {
		/// <summary>
		/// Strongly typed for value GL_INVALID_ENUM. 0x500
		/// </summary>
		InvalidEnum = 0x500,

		/// <summary>
		/// Strongly typed for value GL_INVALID_FRAMEBUFFER_OPERATION, GL_INVALID_FRAMEBUFFER_OPERATION_EXT, 
		/// GL_INVALID_FRAMEBUFFER_OPERATION_OES. 0x506
		/// </summary>
		InvalidFramebufferOperation = 0x506,

		/// <summary>
		/// Strongly typed for value GL_INVALID_OPERATION. 0x502
		/// </summary>
		InvalidOperation = 0x502,

		/// <summary>
		/// Strongly typed for value GL_INVALID_VALUE. 0x501
		/// </summary>
		InvalidValue = 0x501,

		/// <summary>
		/// Strongly typed for value GL_NO_ERROR. 0x000
		/// </summary>
		NoError = 0x000,

		/// <summary>
		/// Strongly typed for value GL_OUT_OF_MEMORY. 0x505
		/// </summary>
		OutOfMemory = 0x505,

		/// <summary>
		/// Strongly typed for value GL_STACK_OVERFLOW. 0x503
		/// </summary>
		StackOverflow = 0x503,

		/// <summary>
		/// Strongly typed for value GL_STACK_UNDERFLOW. 0x504
		/// </summary>
		StackUnderflow = 0x504,

		/// <summary>
		/// Strongly typed for value GL_TABLE_TOO_LARGE, GL_TABLE_TOO_LARGE_EXT. 0x8031
		/// </summary>
		TableTooLarge = 0x8031,

		/// <summary>
		/// Strongly typed for value GL_TEXTURE_TOO_LARGE_EXT. 0x8065
		/// </summary>
		TextureTooLargeExt = 0x8065,

	}
}
