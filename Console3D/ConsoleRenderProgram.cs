﻿using com.RazorSoftware.Logging;
using Console3D.OpenGL;
using Console3D.Textures.TextureAtlas;
using GLFW;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
//using global::OpenGL.Gl = Gl;

namespace Console3D
{
    public class ConsoleRenderProgram : RenderProgram
    {
        protected OpenGL.Shaders.ShaderProgram glyphShader;
        protected Size ConsoleSize = new Size(120, 30);
        protected Textures.TextureAtlas.Atlas fontAtlas;
        protected uint fontAtlasTexture;
        protected Logger KhronosApiLogger;


        public ConsoleRenderProgram(RenderThread renderer) : base(renderer)
        {
        }

        protected override void Renderer_ContextCreated(RenderThread sender, ContextCreationEventArgs args)
        {
            Log.WriteLine("Render context created. Continuing initialization...");

            Log.WriteLine("Running on OpenGL: %@", LogLevel.Message, args.ToString());

            CompileShaders();

            LoadTextures();
        }

        protected override void Renderer_DrawEnd(RenderThread sender, FrameStageEventArgs args)
        {
            
        }

        protected override void Renderer_ProcessingRawInput(RenderThread sender, FrameStageControllerEventArgs args)
        {
            if (Glfw.GetKey(sender.TargetWindow, Keys.Escape) == InputState.Press)
                sender.Stop();
        }

        protected override void Renderer_Draw(RenderThread sender, FrameStageEventArgs args)
        {
            if (!sender.IsMainThread() && !sender.Asynchronous)
                throw new InvalidOperationException("Draw operation initiated from secondary thread when supposed to run in Sync mode.");

            int cellX = sender.InternalResolution.Width / ConsoleSize.Width;
            int cellY = sender.InternalResolution.Height / ConsoleSize.Height;

            uint vao = Gl.GenVertexArray();
            Gl.BindVertexArray(vao);

            CheckGlErrors("pre-texture");

            Gl.ActiveTexture(TextureUnit.Texture0);
            Gl.BindTexture(TextureTarget.Texture2d, fontAtlasTexture);

            CheckGlErrors("post-texture");

            List<float> vertices = new List<float>(4096);
            List<uint> indices = new List<uint>(4096);

            for (int x = 0; x < 1; x++)
            {
                for (int y = 0; y < 1; y++)
                {
                    float ox = x * cellX;
                    float oy = y * cellY;

                    Rectangle texRec = fontAtlas.GetPointerById(65).Bounds;

                    int qi = vertices.Count;

                    vertices.AddRange(new float[] { // Add vertices
                        ox, oy,  // 0,0 (A)
                        texRec.X, texRec.Y, // Texture X,Y
                        1, 0, 0, 1, // Back
                        1, 1, 1, 1, // Fore

                        ox + cellX, oy, // 1,0 (B)
                        texRec.Right, texRec.Y, // Texture X,Y
                        1, 0, 0, 1, // Back
                        1, 1, 1, 1, // Fore

                        ox + cellX, oy + cellY, // 1,1 (C)
                        texRec.Right, texRec.Bottom, // Texture X,Y
                        1, 0, 0, 1, // Back
                        1, 1, 1, 1, // Fore

                        ox, oy + cellY, //0,1 (D)
                        texRec.X, texRec.Bottom, // Texture X,Y
                        1, 0, 0, 1, // Back
                        1, 1, 1, 1, // Fore
                    });


                    indices.AddRange(new uint[]
                    {
                        (uint)(qi+1), // B
                        (uint)(qi+3), // D
                        (uint)(qi), // A
                        //(uint)(qi+1), // B
                        (uint)(qi+2), // C
                        (uint)(qi+3), // D
                    });
                }
            }

            uint vbo = Gl.GenBuffer();
            Gl.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            CheckGlErrors("pre-vab");

            float[] fvertices = vertices.ToArray();
            GCHandle address = GCHandle.Alloc(fvertices, GCHandleType.Pinned);
            Gl.BufferData(BufferTarget.ArrayBuffer, (uint)fvertices.Length * sizeof(float), address.AddrOfPinnedObject(), BufferUsage.StaticDraw);
            address.Free();

            CheckGlErrors("pre-eab");

            uint[] iindices = indices.ToArray();
            address = GCHandle.Alloc(iindices, GCHandleType.Pinned);
            uint ebo = Gl.GenBuffer();
            Gl.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            Gl.BufferData(BufferTarget.ElementArrayBuffer, (uint)iindices.Length * sizeof(float), address.AddrOfPinnedObject(), BufferUsage.StaticDraw);
            address.Free();

            CheckGlErrors("pre_va_pointer");

            Gl.VertexAttribPointer(0, 2, VertexAttribType.Float, false, 12 * sizeof(float), 0 * sizeof(float));
            CheckGlErrors("post-va-pointer-0-set");
            Gl.EnableVertexAttribArray(0);
            CheckGlErrors("post-va-pointer-0-enable");

            Gl.VertexAttribPointer(1, 2, VertexAttribType.Float, false, 12 * sizeof(float), 2 * sizeof(float));
            Gl.EnableVertexAttribArray(1);
            CheckGlErrors("post-va-pointer-1");

            Gl.VertexAttribPointer(2, 4, VertexAttribType.Float, false, 12 * sizeof(float), 4 * sizeof(float));
            Gl.EnableVertexAttribArray(2);
            CheckGlErrors("post-va-pointer-2");

            Gl.VertexAttribPointer(3, 4, VertexAttribType.Float, false, 12 * sizeof(float), 8 * sizeof(float));
            Gl.EnableVertexAttribArray(3);

            CheckGlErrors("predraw");

            Gl.DrawElements(PrimitiveType.TriangleFan, 5, DrawElementsType.UnsignedInt, 0);

            CheckGlErrors("final");
            Gl.BindVertexArray(0);
            CheckGlErrors("unbind");
        }

        protected override void Renderer_DrawPrepare(RenderThread sender, FrameStageControllerEventArgs args)
        {
            CheckGlErrors("pre-program");
            sender.SelectShader(glyphShader);
            CheckGlErrors("post-program");

            sender.SelectShader(glyphShader);
            sender.SetUniform("resolution", 0, 0, sender.InternalResolution.Width, sender.InternalResolution.Height);
            Gl.Uniform1(Gl.GetUniformLocation(glyphShader.ProgramId, "atlasTexture"), 0);
            CheckGlErrors("post-uniforms");
        }

        protected override void LoadTextures()
        {
            Log.WriteLine("Loading textures...");

            Log.WriteLine("Loading font atlas 'Code New Roman'...");
            fontAtlas = Atlas.FromFile(
                FontAtlas.GetAtlasFileFromName("./cache/fonts", "Code New Roman").FullName,
                FontAtlas.GetAtlasMetadataFileFromName("./cache/fonts", "Code New Roman").FullName
            );

            fontAtlasTexture = LoadTexture(fontAtlas.Data, TextureWrapMode.ClampToBorder);
        }

        protected uint LoadTexture(Bitmap data, TextureWrapMode mode)
        {
            BitmapData lockedBitmap = data.LockBits(new Rectangle(Point.Empty, data.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            byte[] buffer = new byte[lockedBitmap.Width * lockedBitmap.Height * 4];

            for (int i = 0; i < buffer.Length; i += 4)
            {
                byte r, g, b, a;
                a = System.Runtime.InteropServices.Marshal.ReadByte(lockedBitmap.Scan0 + i);
                r = System.Runtime.InteropServices.Marshal.ReadByte(lockedBitmap.Scan0 + i + 1);
                g = System.Runtime.InteropServices.Marshal.ReadByte(lockedBitmap.Scan0 + i + 2);
                b = System.Runtime.InteropServices.Marshal.ReadByte(lockedBitmap.Scan0 + i + 3);

                buffer[i] = r;
                buffer[i + 1] = g;
                buffer[i + 2] = b;
                buffer[i + 3] = a;
            }

            data.UnlockBits(lockedBitmap);

#if EMBEDDED_GL
            // TODO: Add embedded GL code here
            uint textureId = Gl.GenTexture();
            Gl.BindTexture(TextureTarget.Texture2d, textureId);
            throw new NotImplementedException();
#else
            CheckGlErrors("pre-texture-gen");
            uint textureId = Gl.GenTexture();
            Gl.ActiveTexture(TextureUnit.Texture0);
            Gl.BindTexture(TextureTarget.Texture2d, textureId);
            Gl.TexParameter(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, (int)mode);
            Gl.TexParameter(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, (int)mode);
            CheckGlErrors("pre-texture-upload");

            Gl.TexImage2D(TextureTarget.Texture2d,
                0,
                InternalFormat.Rgba8,
                data.Width, data.Height,
                0,
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                buffer);
            CheckGlErrors("post-texture-upload");
#endif

            if (mode == TextureWrapMode.ClampToBorder)
            {
                float[] borderColor = new float[] { 0xff, 0x14, 0x93 };
                Gl.TexParameter(TextureTarget.Texture2d, TextureParameterName.TextureBorderColor, borderColor);
            }

            CheckGlErrors("ok-texture-gen");

            return textureId;
        }

        protected void CompileShaders()
        {
            Log.WriteLine("Compiling shaders...");
            Log.WriteLine("Compiling 'glyph' program...");
            glyphShader = OpenGL.Shaders.ShaderProgram.FromFiles("./shaders/glyph.vert", "./shaders/glyph.frag");
            glyphShader.Compile();
            Log.WriteLine("Shader compilation completed.");
        }
    }
}
