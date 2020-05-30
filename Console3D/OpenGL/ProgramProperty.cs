#if EMBEDDED_GL
using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D.OpenGL
{
	// Requires GL3.2
    public enum ProgramProperty
    {
		/// <summary>
		/// Strongly typed for value GL_DELETE_STATUS. 0x8B80
		/// </summary>
		DeleteStatus = 0x8B80,

		/// <summary>
		/// Strongly typed for value GL_LINK_STATUS. 0x8B82
		/// </summary>
		LinkStatus = 0x8B82,

		/// <summary>
		/// Strongly typed for value GL_VALIDATE_STATUS. 0x8B83
		/// </summary>
		ValidateStatus = 0x8B83,

		/// <summary>
		/// Strongly typed for value GL_INFO_LOG_LENGTH. 0x8B84
		/// </summary>
		InfoLogLength = 0x8B84,

		/// <summary>
		/// Strongly typed for value GL_ATTACHED_SHADERS. 0x8B85
		/// </summary>
		AttachedShaders = 0x8B85,

		/// <summary>
		/// Strongly typed for value GL_ACTIVE_ATTRIBUTES. 0x8B89
		/// </summary>
		ActiveAttributes = 0x8B89,

		/// <summary>
		/// Strongly typed for value GL_ACTIVE_ATTRIBUTE_MAX_LENGTH. 0x8B8A
		/// </summary>
		ActiveAttributeMaxLength = 0x8B8A,

		/// <summary>
		/// Strongly typed for value GL_ACTIVE_UNIFORMS. 0x8B86
		/// </summary>
		ActiveUniforms = 0x8B86,

		/// <summary>
		/// Strongly typed for value GL_ACTIVE_UNIFORM_BLOCKS. 0x8A36
		/// </summary>
		ActiveUniformBlocks = 0x8A36,

		/// <summary>
		/// Strongly typed for value GL_ACTIVE_UNIFORM_BLOCK_MAX_NAME_LENGTH. 0x8A35
		/// </summary>
		ActiveUniformBlockMaxNameLength = 0x8A35,

		/// <summary>
		/// Strongly typed for value GL_ACTIVE_UNIFORM_MAX_LENGTH. 0x8B87
		/// </summary>
		ActiveUniformMaxLength = 0x8B87,

		/// <summary>
		/// Strongly typed for value GL_TRANSFORM_FEEDBACK_BUFFER_MODE. 0x8C7F
		/// </summary>
		TransformFeedbackBufferMode = 0x8C7F,

		/// <summary>
		/// Strongly typed for value GL_TRANSFORM_FEEDBACK_VARYINGS. 0x8C83
		/// </summary>
		TransformFeedbackVaryings = 0x8C83,

		/// <summary>
		/// Strongly typed for value GL_TRANSFORM_FEEDBACK_VARYING_MAX_LENGTH. 0x8C76
		/// </summary>
		TransformFeedbackVaryingMaxLength = 0x8C76,

		/// <summary>
		/// Strongly typed for value GL_GEOMETRY_VERTICES_OUT. 0x8916
		/// </summary>
		GeometryVerticesOut = 0x8916,

		/// <summary>
		/// Strongly typed for value GL_GEOMETRY_INPUT_TYPE. 0x8917
		/// </summary>
		GeometryInputType = 0x8917,

		/// <summary>
		/// Strongly typed for value GL_GEOMETRY_OUTPUT_TYPE. 0x8918
		/// </summary>
		GeometryOutputType = 0x8918,
	}
}
#endif