using com.RazorSoftware.Logging;
using Console3D.OpenGL;
using Console3D.Textures.TextureAtlas;
using OpenToolkit.Graphics.OpenGL;
using OpenToolkit.Windowing.Common.Input;
using OpenToolkit.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Gl = OpenToolkit.Graphics.OpenGL.GL;
using PixelFormat = OpenToolkit.Graphics.OpenGL.PixelFormat;

namespace Console3D
{
    public class ConsoleRenderProgram : RenderProgram
    {
        protected Size ConsoleSize = new Size(120, 30);
        protected Atlas fontAtlas;
        protected int fontAtlasTexture;
        protected OpenGL.Shaders.ShaderProgram glyphShader;
        protected Logger KhronosApiLogger;

        public ConsoleRenderProgram(RenderThread renderer) : base(renderer)
        {
            Renderer.AutoEnableCaps = AutoEnableCapabilitiesFlags.Blend;

            Glyphs = new RenderGlyph[ConsoleSize.Width * ConsoleSize.Height];

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                BackColor = Color.Black;
                ForeColor = Color.DarkGray;
            }
        }

        private void TargetWindow_KeyUp(OpenToolkit.Windowing.Common.KeyboardKeyEventArgs obj)
        {
            char c = (char)(obj.Key - 19);
            string cval = ((int)obj.Key).ToString("X2");
            Log.WriteLine($"0x{cval} {obj.Key} -> {c}");

            if (obj.Key == Key.Enter || obj.Key == Key.KeypadEnter)
            {
                WriteLine();
                return;
            }

            if (obj.Key == Key.BackSpace)
            {
                if (CursorLeft < 1)
                    return;

                CursorLeft--;
                SetGlyph(Cursor.X, Cursor.Y, null);
                return;
            }

            char? character = KeyConverter.KeyToChar(obj.Key, Renderer.TargetWindow.KeyboardState.IsKeyDown(Key.CapsLock), obj.Shift);

            if (character.HasValue)
                Write(new string(character.Value, 1));
        }

        public Color BackColor { get; set; }
        public Point Cursor { get; set; }
        public int CursorLeft
        {
            get
            {
                return Cursor.X;
            }
            set
            {
                Cursor = new Point(value, Cursor.Y);
            }
        }

        public int CursorTop
        {
            get
            {
                return Cursor.Y;
            }
            set
            {
                Cursor = new Point(Cursor.X, value);
            }
        }

        public Color ForeColor { get; set; }
        protected RenderGlyph[] Glyphs { get; set; }
        public void Clear()
        {
            for (int i = 0; i < Glyphs.Length; i++)
                Glyphs[i] = null;
        }

        public RenderGlyph GetGlyph(int x, int y)
        {
            return Glyphs[GetGlyphAddress(x, y)];
        }

        public void SetGlyph(int x, int y, RenderGlyph glyph)
        {
            Glyphs[GetGlyphAddress(x, y)] = glyph;
        }

        public void SetGlyph(int x, int y, char value)
        {
            Glyphs[GetGlyphAddress(x, y)] = new RenderGlyph(value);
        }

        protected float[] ColorToFloatArray(Color c)
        {
            return new float[]
            {
                c.R / 255.0f,
                c.G / 255.0f,
                c.B / 255.0f,
                c.A / 255.0f,
            };
        }

        protected void CompileShaders()
        {
            Log.WriteLine("Compiling shaders...");
            Log.WriteLine("Compiling 'glyph' program...");
            glyphShader = OpenGL.Shaders.ShaderProgram.FromFiles("./shaders/glyph.vert", "./shaders/glyph.frag");
            glyphShader.Compile();
            Log.WriteLine("Shader compilation completed.");
        }

        protected int GetGlyphAddress(int x, int y)
        {
            return x + (y * ConsoleSize.Width);
        }
        protected int LoadTexture(Bitmap data, TextureWrapMode mode)
        {
            BitmapData lockedBitmap = data.LockBits(new Rectangle(Point.Empty, data.Size), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            byte[] buffer = new byte[lockedBitmap.Width * lockedBitmap.Height * 4];

            for (int i = 0; i < buffer.Length; i += 4)
            {
                byte r, g, b, a;
                a = Marshal.ReadByte(lockedBitmap.Scan0 + i);
                r = Marshal.ReadByte(lockedBitmap.Scan0 + i + 1);
                g = Marshal.ReadByte(lockedBitmap.Scan0 + i + 2);
                b = Marshal.ReadByte(lockedBitmap.Scan0 + i + 3);

                buffer[i] = r;
                buffer[i + 1] = g;
                buffer[i + 2] = b;
                buffer[i + 3] = a;
            }

            data.UnlockBits(lockedBitmap);

            CheckGlErrors("pre-texture-gen");
            int textureId = Gl.GenTexture();
            Gl.ActiveTexture(TextureUnit.Texture0);
            Gl.BindTexture(TextureTarget.Texture2D, textureId);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)mode);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)mode);
            CheckGlErrors("pre-texture-upload");

            Gl.TexImage2D(TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba8,
                data.Width, data.Height,
                0,
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                buffer);
            CheckGlErrors("post-texture-upload");

            if (mode == TextureWrapMode.ClampToBorder)
            {
                float[] borderColor = new float[] { 0xff, 0x14, 0x93 };
                Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, borderColor);
            }

            CheckGlErrors("ok-texture-gen");

            return textureId;
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

        protected override void Renderer_ContextCreated(RenderThread sender, ContextCreationEventArgs args)
        {
            Log.WriteLine("Render context created. Continuing initialization...");

            Log.WriteLine("Running on OpenGL: %@", LogLevel.Message, Gl.GetString(StringName.Renderer) + Gl.GetString(StringName.Version));

            Renderer.TargetWindow.KeyUp += TargetWindow_KeyUp;

            CompileShaders();

            LoadTextures();

            Gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }

        protected override void Renderer_Draw(RenderThread sender, FrameStageEventArgs args)
        {
            if (!sender.IsMainThread() && !sender.Asynchronous)
                throw new InvalidOperationException("Draw operation initiated from secondary thread when supposed to run in Sync mode.");

            float cellX = sender.InternalResolution.Width / (float)ConsoleSize.Width;
            float cellY = sender.InternalResolution.Height / (float)ConsoleSize.Height;

            int vao = Gl.GenVertexArray();
            Gl.BindVertexArray(vao);

            CheckGlErrors("pre-texture");

            Gl.ActiveTexture(TextureUnit.Texture0);
            Gl.BindTexture(TextureTarget.Texture2D, fontAtlasTexture);

            CheckGlErrors("post-texture");

            List<float> vertices = new List<float>(4096);
            List<uint> indices = new List<uint>(4096);

            float[] db = ColorToFloatArray(BackColor);
            float[] df = ColorToFloatArray(ForeColor);

            int vertexCount = 0;

            for (int xi = 0; xi < ConsoleSize.Width; xi++)
            {
                float x = xi * cellX;
                for (int yi = 0; yi < ConsoleSize.Height; yi++)
                {
                    RenderGlyph glyphData = GetGlyph(xi, yi);

                    float y = yi * cellY;
                    Rectangle texRec = fontAtlas.GetPointerById((int)(glyphData?.Glyph ?? ' ')).Bounds;

                    float[] b = glyphData == null || !glyphData.Background.HasValue ? db : ColorToFloatArray(glyphData.Background.Value);
                    float[] f = glyphData == null || !glyphData.Foreground.HasValue ? df : ColorToFloatArray(glyphData.Foreground.Value);

                    int qi = vertexCount;

                    vertices.AddRange(new float[] { // Add vertices
                        x, y,  // 0,0 (A)
                        texRec.X, texRec.Y, // Texture X,Y
                        b[0], b[1], b[2], b[3], // Back
                        f[0], f[1], f[2], f[3], // Fore

                        x + cellX, y, // 1,0 (B)
                        texRec.Right, texRec.Y, // Texture X,Y
                        b[0], b[1], b[2], b[3], // Back
                        f[0], f[1], f[2], f[3], // Fore

                        x + cellX, y + cellY, // 1,1 (C)
                        texRec.Right, texRec.Bottom, // Texture X,Y
                        b[0], b[1], b[2], b[3], // Back
                        f[0], f[1], f[2], f[3], // Fore

                        x, y + cellY, //0,1 (D)
                        texRec.X, texRec.Bottom, // Texture X,Y
                        b[0], b[1], b[2], b[3], // Back
                        f[0], f[1], f[2], f[3], // Fore
                    });

                    indices.AddRange(new uint[]
                    {
                        (uint)(qi+1), // B
                        (uint)(qi+3), // D
                        (uint)(qi), // A
                        (uint)(qi+1), // B
                        (uint)(qi+2), // C
                        (uint)(qi+3), // D
                    });

                    vertexCount += 4;
                }
            }

            int vbo = Gl.GenBuffer();
            Gl.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            CheckGlErrors("pre-vab");

            float[] fvertices = vertices.ToArray();
            GCHandle buffAddress = GCHandle.Alloc(fvertices, GCHandleType.Pinned);
            Gl.BufferData(BufferTarget.ArrayBuffer, fvertices.Length * sizeof(float), buffAddress.AddrOfPinnedObject(), BufferUsageHint.StaticDraw);
            buffAddress.Free();

            CheckGlErrors("pre-eab");

            uint[] iindices = indices.ToArray();
            buffAddress = GCHandle.Alloc(iindices, GCHandleType.Pinned);
            int ebo = Gl.GenBuffer();
            Gl.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            Gl.BufferData(BufferTarget.ElementArrayBuffer, iindices.Length * sizeof(float), buffAddress.AddrOfPinnedObject(), BufferUsageHint.StaticDraw);
            buffAddress.Free();

            CheckGlErrors("pre_va_pointer");

            Gl.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 12 * sizeof(float), 0 * sizeof(float));
            CheckGlErrors("post-va-pointer-0-set");
            Gl.EnableVertexAttribArray(0);
            CheckGlErrors("post-va-pointer-0-enable");

            Gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 12 * sizeof(float), 2 * sizeof(float));
            Gl.EnableVertexAttribArray(1);
            CheckGlErrors("post-va-pointer-1");

            Gl.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 12 * sizeof(float), 4 * sizeof(float));
            Gl.EnableVertexAttribArray(2);
            CheckGlErrors("post-va-pointer-2");

            Gl.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, 12 * sizeof(float), 8 * sizeof(float));
            Gl.EnableVertexAttribArray(3);

            CheckGlErrors("predraw");

            Gl.DrawElements(PrimitiveType.Triangles, iindices.Length, DrawElementsType.UnsignedInt, 0);

            CheckGlErrors("final");
            Gl.BindVertexArray(0);
            CheckGlErrors("unbind");
        }

        protected override void Renderer_DrawEnd(RenderThread sender, FrameStageEventArgs args)
        {
        }

        protected override void Renderer_DrawPrepare(RenderThread sender, FrameStageControllerEventArgs args)
        {
            CheckGlErrors("pre-program");
            sender.SelectShader(glyphShader);
            CheckGlErrors("post-program");

            sender.SelectShader(glyphShader);
            sender.SetUniform("resolution", 0, 0, sender.InternalResolution.Width, sender.InternalResolution.Height);
            sender.SetUniform("atlasSize", fontAtlas.AtlasTextureSize.Width, fontAtlas.AtlasTextureSize.Height);
            Gl.Uniform1(Gl.GetUniformLocation(glyphShader.ProgramId, "atlasTexture"), 0);
            CheckGlErrors("post-uniforms");
        }

        protected override void Renderer_ProcessingRawInput(RenderThread sender, FrameStageControllerEventArgs args)
        {
            if (Renderer.TargetWindow.KeyboardState.IsKeyDown(OpenToolkit.Windowing.Common.Input.Key.Escape))
            {
                args.AbortExecution = true;
                return;
            }
        }

        public void WriteLine(string text)
        {
            Write(text);
            WriteLine();
        }

        public void WriteLine()
        {
            Cursor = new Point(0, Cursor.Y + 1);
        }
        
        public void Write(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (Cursor.X >= ConsoleSize.Width)
                    Cursor = new Point(0, Cursor.Y + 1);

                int address = GetGlyphAddress(Cursor.X, Cursor.Y);
                Glyphs[address] = new RenderGlyph(text[i]);

                Cursor = new Point(Cursor.X + 1, Cursor.Y);
            }
        }
    }
}