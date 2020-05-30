#if EMBEDDED_GL
using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D.OpenGL
{
	// Requires GL3.1
    public enum EnableCap
    {
        ///// <summary>
        ///// Strongly typed for value GL_ALPHA_TEST.
        ///// </summary>
        //AlphaTest = Gl.ALPHA_TEST,

        ///// <summary>
        ///// Strongly typed for value GL_AUTO_NORMAL.
        ///// </summary>
        //AutoNormal = Gl.AUTO_NORMAL,

        /// <summary>
        /// Strongly typed for value GL_BLEND. 0x0BE2
        /// </summary>
        Blend = 0x0BE2,

        ///// <summary>
        ///// Strongly typed for value GL_CLIP_PLANE0, GL_CLIP_DISTANCE0.
        ///// </summary>
        //ClipPlane0 = Gl.CLIP_PLANE0,

        ///// <summary>
        ///// Strongly typed for value GL_CLIP_PLANE0, GL_CLIP_DISTANCE0.
        ///// </summary>
        //ClipDistance0 = Gl.CLIP_DISTANCE0,

        ///// <summary>
        ///// Strongly typed for value GL_CLIP_PLANE1, GL_CLIP_DISTANCE1.
        ///// </summary>
        //ClipPlane1 = Gl.CLIP_PLANE1,

        ///// <summary>
        ///// Strongly typed for value GL_CLIP_PLANE1, GL_CLIP_DISTANCE1.
        ///// </summary>
        //ClipDistance1 = Gl.CLIP_DISTANCE1,

        ///// <summary>
        ///// Strongly typed for value GL_CLIP_PLANE2, GL_CLIP_DISTANCE2.
        ///// </summary>
        //ClipPlane2 = Gl.CLIP_PLANE2,

        ///// <summary>
        ///// Strongly typed for value GL_CLIP_PLANE2, GL_CLIP_DISTANCE2.
        ///// </summary>
        //ClipDistance2 = Gl.CLIP_DISTANCE2,

        ///// <summary>
        ///// Strongly typed for value GL_CLIP_PLANE3, GL_CLIP_DISTANCE3.
        ///// </summary>
        //ClipPlane3 = Gl.CLIP_PLANE3,

        ///// <summary>
        ///// Strongly typed for value GL_CLIP_PLANE3, GL_CLIP_DISTANCE3.
        ///// </summary>
        //ClipDistance3 = Gl.CLIP_DISTANCE3,

        ///// <summary>
        ///// Strongly typed for value GL_CLIP_PLANE4, GL_CLIP_DISTANCE4.
        ///// </summary>
        //ClipPlane4 = Gl.CLIP_PLANE4,

        ///// <summary>
        ///// Strongly typed for value GL_CLIP_PLANE4, GL_CLIP_DISTANCE4.
        ///// </summary>
        //ClipDistance4 = Gl.CLIP_DISTANCE4,

        ///// <summary>
        ///// Strongly typed for value GL_CLIP_PLANE5, GL_CLIP_DISTANCE5.
        ///// </summary>
        //ClipPlane5 = Gl.CLIP_PLANE5,

        ///// <summary>
        ///// Strongly typed for value GL_CLIP_PLANE5, GL_CLIP_DISTANCE5.
        ///// </summary>
        //ClipDistance5 = Gl.CLIP_DISTANCE5,

        ///// <summary>
        ///// Strongly typed for value GL_CLIP_DISTANCE6.
        ///// </summary>
        //ClipDistance6 = Gl.CLIP_DISTANCE6,

        ///// <summary>
        ///// Strongly typed for value GL_CLIP_DISTANCE7.
        ///// </summary>
        //ClipDistance7 = Gl.CLIP_DISTANCE7,

        ///// <summary>
        ///// Strongly typed for value GL_COLOR_ARRAY.
        ///// </summary>
        //ColorArray = Gl.COLOR_ARRAY,

        ///// <summary>
        ///// Strongly typed for value GL_COLOR_LOGIC_OP.
        ///// </summary>
        //ColorLogicOp = Gl.COLOR_LOGIC_OP,

        ///// <summary>
        ///// Strongly typed for value GL_COLOR_MATERIAL.
        ///// </summary>
        //ColorMaterial = Gl.COLOR_MATERIAL,

        ///// <summary>
        ///// Strongly typed for value GL_COLOR_TABLE_SGI.
        ///// </summary>
        //ColorTable = Gl.COLOR_TABLE,

        ///// <summary>
        ///// Strongly typed for value GL_CULL_FACE.
        ///// </summary>
        //CullFace = Gl.CULL_FACE,

        /// <summary>
        /// Strongly typed for value GL_DEPTH_TEST. 0x0B71
        /// </summary>
        DepthTest = 0x0B71,

        /// <summary>
        /// Strongly typed for value GL_DITHER. 0x0BD0
        /// </summary>
        Dither = 0x0BD0,

        ///// <summary>
        ///// Strongly typed for value GL_EDGE_FLAG_ARRAY.
        ///// </summary>
        //EdgeFlagArray = Gl.EDGE_FLAG_ARRAY,

        ///// <summary>
        ///// Strongly typed for value GL_FOG.
        ///// </summary>
        //Fog = Gl.FOG,

        ///// <summary>
        ///// Strongly typed for value GL_INDEX_ARRAY.
        ///// </summary>
        //IndexArray = Gl.INDEX_ARRAY,

        ///// <summary>
        ///// Strongly typed for value GL_INDEX_LOGIC_OP.
        ///// </summary>
        //IndexLogicOp = Gl.INDEX_LOGIC_OP,

        ///// <summary>
        ///// Strongly typed for value GL_LIGHT0.
        ///// </summary>
        //Light0 = Gl.LIGHT0,

        ///// <summary>
        ///// Strongly typed for value GL_LIGHT1.
        ///// </summary>
        //Light1 = Gl.LIGHT1,

        ///// <summary>
        ///// Strongly typed for value GL_LIGHT2.
        ///// </summary>
        //Light2 = Gl.LIGHT2,

        ///// <summary>
        ///// Strongly typed for value GL_LIGHT3.
        ///// </summary>
        //Light3 = Gl.LIGHT3,

        ///// <summary>
        ///// Strongly typed for value GL_LIGHT4.
        ///// </summary>
        //Light4 = Gl.LIGHT4,

        ///// <summary>
        ///// Strongly typed for value GL_LIGHT5.
        ///// </summary>
        //Light5 = Gl.LIGHT5,

        ///// <summary>
        ///// Strongly typed for value GL_LIGHT6.
        ///// </summary>
        //Light6 = Gl.LIGHT6,

        ///// <summary>
        ///// Strongly typed for value GL_LIGHT7.
        ///// </summary>
        //Light7 = Gl.LIGHT7,

        ///// <summary>
        ///// Strongly typed for value GL_LIGHTING.
        ///// </summary>
        //Lighting = Gl.LIGHTING,

        /// <summary>
        /// Strongly typed for value GL_LINE_SMOOTH. 0x0B20
        /// </summary>
        LineSmooth = 0x0B20,

        /// <summary>
        /// Strongly typed for value GL_LINE_STIPPLE. 0x0B24
        /// </summary>
        LineStipple = 0x0B24,

        ///// <summary>
        ///// Strongly typed for value GL_MAP1_COLOR_4.
        ///// </summary>
        //Map1Color4 = Gl.MAP1_COLOR_4,

        ///// <summary>
        ///// Strongly typed for value GL_MAP1_INDEX.
        ///// </summary>
        //Map1Index = Gl.MAP1_INDEX,

        ///// <summary>
        ///// Strongly typed for value GL_MAP1_NORMAL.
        ///// </summary>
        //Map1Normal = Gl.MAP1_NORMAL,

        ///// <summary>
        ///// Strongly typed for value GL_MAP1_TEXTURE_COORD_1.
        ///// </summary>
        //Map1TextureCoord1 = Gl.MAP1_TEXTURE_COORD_1,

        ///// <summary>
        ///// Strongly typed for value GL_MAP1_TEXTURE_COORD_2.
        ///// </summary>
        //Map1TextureCoord2 = Gl.MAP1_TEXTURE_COORD_2,

        ///// <summary>
        ///// Strongly typed for value GL_MAP1_TEXTURE_COORD_3.
        ///// </summary>
        //Map1TextureCoord3 = Gl.MAP1_TEXTURE_COORD_3,

        ///// <summary>
        ///// Strongly typed for value GL_MAP1_TEXTURE_COORD_4.
        ///// </summary>
        //Map1TextureCoord4 = Gl.MAP1_TEXTURE_COORD_4,

        ///// <summary>
        ///// Strongly typed for value GL_MAP1_VERTEX_3.
        ///// </summary>
        //Map1Vertex3 = Gl.MAP1_VERTEX_3,

        ///// <summary>
        ///// Strongly typed for value GL_MAP1_VERTEX_4.
        ///// </summary>
        //Map1Vertex4 = Gl.MAP1_VERTEX_4,

        ///// <summary>
        ///// Strongly typed for value GL_MAP2_COLOR_4.
        ///// </summary>
        //Map2Color4 = Gl.MAP2_COLOR_4,

        ///// <summary>
        ///// Strongly typed for value GL_MAP2_INDEX.
        ///// </summary>
        //Map2Index = Gl.MAP2_INDEX,

        ///// <summary>
        ///// Strongly typed for value GL_MAP2_NORMAL.
        ///// </summary>
        //Map2Normal = Gl.MAP2_NORMAL,

        ///// <summary>
        ///// Strongly typed for value GL_MAP2_TEXTURE_COORD_1.
        ///// </summary>
        //Map2TextureCoord1 = Gl.MAP2_TEXTURE_COORD_1,

        ///// <summary>
        ///// Strongly typed for value GL_MAP2_TEXTURE_COORD_2.
        ///// </summary>
        //Map2TextureCoord2 = Gl.MAP2_TEXTURE_COORD_2,

        ///// <summary>
        ///// Strongly typed for value GL_MAP2_TEXTURE_COORD_3.
        ///// </summary>
        //Map2TextureCoord3 = Gl.MAP2_TEXTURE_COORD_3,

        ///// <summary>
        ///// Strongly typed for value GL_MAP2_TEXTURE_COORD_4.
        ///// </summary>
        //Map2TextureCoord4 = Gl.MAP2_TEXTURE_COORD_4,

        ///// <summary>
        ///// Strongly typed for value GL_MAP2_VERTEX_3.
        ///// </summary>
        //Map2Vertex3 = Gl.MAP2_VERTEX_3,

        ///// <summary>
        ///// Strongly typed for value GL_MAP2_VERTEX_4.
        ///// </summary>
        //Map2Vertex4 = Gl.MAP2_VERTEX_4,

        ///// <summary>
        ///// Strongly typed for value GL_NORMALIZE.
        ///// </summary>
        //Normalize = Gl.NORMALIZE,

        ///// <summary>
        ///// Strongly typed for value GL_NORMAL_ARRAY.
        ///// </summary>
        //NormalArray = Gl.NORMAL_ARRAY,

        ///// <summary>
        ///// Strongly typed for value GL_POINT_SMOOTH.
        ///// </summary>
        //PointSmooth = Gl.POINT_SMOOTH,

        ///// <summary>
        ///// Strongly typed for value GL_POLYGON_OFFSET_FILL.
        ///// </summary>
        //PolygonOffsetFill = Gl.POLYGON_OFFSET_FILL,

        ///// <summary>
        ///// Strongly typed for value GL_POLYGON_OFFSET_LINE.
        ///// </summary>
        //PolygonOffsetLine = Gl.POLYGON_OFFSET_LINE,

        ///// <summary>
        ///// Strongly typed for value GL_POLYGON_OFFSET_POINT.
        ///// </summary>
        //PolygonOffsetPoint = Gl.POLYGON_OFFSET_POINT,

        ///// <summary>
        ///// Strongly typed for value GL_POLYGON_SMOOTH.
        ///// </summary>
        //PolygonSmooth = Gl.POLYGON_SMOOTH,

        ///// <summary>
        ///// Strongly typed for value GL_POLYGON_STIPPLE.
        ///// </summary>
        //PolygonStipple = Gl.POLYGON_STIPPLE,

        ///// <summary>
        ///// Strongly typed for value GL_RESCALE_NORMAL_EXT.
        ///// </summary>
        //RescaleNormal = Gl.RESCALE_NORMAL,

        ///// <summary>
        ///// Strongly typed for value GL_SAMPLE_ALPHA_TO_ONE_SGIS.
        ///// </summary>
        //SampleAlphaToOne = Gl.SAMPLE_ALPHA_TO_ONE,

        ///// <summary>
        ///// Strongly typed for value GL_SCISSOR_TEST.
        ///// </summary>
        //ScissorTest = Gl.SCISSOR_TEST,

        ///// <summary>
        ///// Strongly typed for value GL_SEPARABLE_2D_EXT.
        ///// </summary>
        //Separable2d = Gl.SEPARABLE_2D,

        ///// <summary>
        ///// Strongly typed for value GL_SHARED_TEXTURE_PALETTE_EXT.
        ///// </summary>
        //SharedTexturePaletteExt = Gl.SHARED_TEXTURE_PALETTE_EXT,

        ///// <summary>
        ///// Strongly typed for value GL_STENCIL_TEST.
        ///// </summary>
        //StencilTest = Gl.STENCIL_TEST,

        /// <summary>
        /// Strongly typed for value GL_TEXTURE_1D. 0x0DE0
        /// </summary>
        Texture1d = 0x0DE0,

        /// <summary>
        /// Strongly typed for value GL_TEXTURE_2D. 0x0DE1
        /// </summary>
        Texture2d = 0x0DE1,

        /// <summary>
        /// Strongly typed for value GL_TEXTURE_3D_EXT. 0x806F
        /// </summary>
        Texture3d = 0x806F,

        ///// <summary>
        ///// Strongly typed for value GL_TEXTURE_COORD_ARRAY.
        ///// </summary>
        //TextureCoordArray = Gl.TEXTURE_COORD_ARRAY,

        ///// <summary>
        ///// Strongly typed for value GL_TEXTURE_GEN_Q.
        ///// </summary>
        //TextureGenQ = Gl.TEXTURE_GEN_Q,

        ///// <summary>
        ///// Strongly typed for value GL_TEXTURE_GEN_R.
        ///// </summary>
        //TextureGenR = Gl.TEXTURE_GEN_R,

        ///// <summary>
        ///// Strongly typed for value GL_TEXTURE_GEN_S.
        ///// </summary>
        //TextureGenS = Gl.TEXTURE_GEN_S,

        ///// <summary>
        ///// Strongly typed for value GL_TEXTURE_GEN_T.
        ///// </summary>
        //TextureGenT = Gl.TEXTURE_GEN_T,

        ///// <summary>
        ///// Strongly typed for value GL_VERTEX_ARRAY.
        ///// </summary>
        //VertexArray = Gl.VERTEX_ARRAY,

        ///// <summary>
        ///// Strongly typed for value GL_FRAMEBUFFER_SRGB.
        ///// </summary>
        //FramebufferSrgb = Gl.FRAMEBUFFER_SRGB,

        ///// <summary>
        ///// Strongly typed for value GL_PRIMITIVE_RESTART.
        ///// </summary>
        //PrimitiveRestart = Gl.PRIMITIVE_RESTART,

        ///// <summary>
        ///// Strongly typed for value GL_PRIMITIVE_RESTART_NV.
        ///// </summary>
        //PrimitiveRestartNv = Gl.PRIMITIVE_RESTART_NV,

        ///// <summary>
        ///// Strongly typed for value GL_RASTERIZER_DISCARD.
        ///// </summary>
        //RasterizerDiscard = Gl.RASTERIZER_DISCARD,

    }
}
#endif