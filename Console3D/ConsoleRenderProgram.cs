using com.RazorSoftware.Logging;
using Console3D.OpenGL;
using Console3D.Textures.Text;
using Console3D.Textures.TextureAtlas;
using OpenToolkit.Graphics.OpenGL;
using OpenToolkit.Windowing.Common.Input;
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
        public Size ConsoleSize = new Size(120, 30);
        protected Atlas fontAtlas;
        protected int fontAtlasTexture;
        protected OpenGL.Shaders.ShaderProgram glyphShader;
        protected Logger KhronosApiLogger;
        protected int VAO;
        protected int VBO;
        protected int EBO;

        public delegate void ConsoleRenderProgramKeyEventHandler(ConsoleRenderProgram sender, ConsoleRenderProgramKeyEventArgs e);

        public event ConsoleRenderProgramKeyEventHandler KeyUp;
        public event ConsoleRenderProgramKeyEventHandler KeyDown;

        public ConsoleRenderProgram(RenderThread renderer) : base(renderer)
        {
            Renderer.AutoEnableCaps = AutoEnableCapabilitiesFlags.Blend;

            Glyphs = new CircularBuffer<RenderGlyph[]>(ConsoleSize.Height);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                DefaultBackColor = Color.Black;
                DefaultForeColor = Color.LightGray;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                DefaultBackColor = Color.White;
                DefaultForeColor = Color.DarkSlateGray;
            }
            else
            {
                DefaultBackColor = Color.Black;
                DefaultForeColor = Color.LightGreen;
            }

            BackColor = DefaultBackColor;
            ForeColor = DefaultForeColor;
        }

        public override void Run()
        {
            Glyphs = new CircularBuffer<RenderGlyph[]>(ConsoleSize.Height);

            base.Run();
        }

        public Color DefaultBackColor { get; set; }

        public float BlinkInterval { get; set; } = 500; // Milliseconds

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

        public bool ShowCursor { get; set; } = true;

        public string FontName { get; set; } = "Code New Roman";

        
        public Color DefaultForeColor { get; set; }

        public Color BackColor { get; set; }
        public Color ForeColor { get; set; }
        public bool InheritDefaultColors { get; set; } = true;

        protected CircularBuffer<RenderGlyph[]> Glyphs { get; set; }

        public void Clear()
        {
            Glyphs = new CircularBuffer<RenderGlyph[]>(ConsoleSize.Height);
        }

        public RenderGlyph GetGlyph(int x, int y)
        {
            return Glyphs.ElementAt(y)?[x] ?? null;
        }

        public void SetGlyph(int x, int y, RenderGlyph glyph)
        {
            if (Glyphs[y] == null)
                Glyphs[y] = new RenderGlyph[ConsoleSize.Width];

            Glyphs[y][x] = glyph;
        }

        public void SetGlyph(int x, int y, char value)
        {
            if (Glyphs[y] == null)
                Glyphs[y] = new RenderGlyph[ConsoleSize.Width];

            Glyphs[y][x] = new RenderGlyph(value);
        }

        public void Write(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (Cursor.X >= ConsoleSize.Width)
                    Cursor = new Point(0, Cursor.Y + 1);
                if (Cursor.Y >= ConsoleSize.Height)
                    Cursor = new Point(Cursor.X, ConsoleSize.Height - 1);

                RenderGlyph glyph = new RenderGlyph(text[i]);

                if (InheritDefaultColors)
                {
                    glyph.Background = BackColor;
                    glyph.Foreground = ForeColor;
                }

                SetGlyph(Cursor.X, Cursor.Y, glyph);

                if (Cursor.X + 1 < ConsoleSize.Width)
                    Cursor = new Point(Cursor.X + 1, Cursor.Y);
                else
                    Cursor = new Point(0, Cursor.Y + 1);

                if (Cursor.Y >= ConsoleSize.Height)
                {
                    Cursor = new Point(Cursor.X, ConsoleSize.Height - 1);
                    Glyphs.Rotate(1);
                    if (Glyphs[Cursor.Y] != null)
                    {
                        for (int w = 0; w < Glyphs[Cursor.Y].Length; w++)
                            Glyphs[Cursor.Y][w] = null;
                    }
                }
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

            if (Cursor.Y >= ConsoleSize.Height)
            {
                Cursor = new Point(Cursor.X, ConsoleSize.Height - 1);
                Glyphs.Rotate(1);
                if (Glyphs[Cursor.Y] != null)
                {
                    for (int w = 0; w < Glyphs[Cursor.Y].Length; w++)
                        Glyphs[Cursor.Y][w] = null;
                }
            }
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
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
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

            Log.WriteLine($"Loading font atlas '{FontName}'...");
            fontAtlas = Atlas.FromFile(
                FontAtlas.GetAtlasFileFromName("./cache/fonts", FontName).FullName,
                FontAtlas.GetAtlasMetadataFileFromName("./cache/fonts", FontName).FullName
            );

            fontAtlasTexture = LoadTexture(fontAtlas.Data, TextureWrapMode.ClampToBorder);
        }

        protected override void Renderer_ContextCreated(RenderThread sender, ContextCreationEventArgs args)
        {
            Log.WriteLine("Render context created. Continuing initialization...");

            Log.WriteLine("Running on OpenGL: %@", LogLevel.Message, Gl.GetString(StringName.Renderer) + Gl.GetString(StringName.Version));

            Renderer.TargetWindow.KeyDown += TargetWindow_KeyDown;
            Renderer.TargetWindow.KeyUp += TargetWindow_KeyUp;

            CompileShaders();

            LoadTextures();

            Gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            VAO = Gl.GenVertexArray();
            Gl.BindVertexArray(VAO);

            CheckGlErrors("pre-texture");

            Gl.ActiveTexture(TextureUnit.Texture0);
            Gl.BindTexture(TextureTarget.Texture2D, fontAtlasTexture);

            CheckGlErrors("post-texture");

            VBO = Gl.GenBuffer();
            Gl.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            EBO = Gl.GenBuffer();
            Gl.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);

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
        }

        private void TargetWindow_KeyUp(OpenToolkit.Windowing.Common.KeyboardKeyEventArgs obj)
        {
            ConsoleRenderProgramKeyEventArgs args = new ConsoleRenderProgramKeyEventArgs(obj);

            OnKeyUp(args);
        }

        protected override void Renderer_Draw(RenderThread sender, FrameStageEventArgs args)
        {
            if (!sender.IsMainThread() && !sender.Asynchronous)
                throw new InvalidOperationException("Draw operation initiated from secondary thread when supposed to run in Sync mode.");

            float cellX = sender.InternalResolution.Width / (float)ConsoleSize.Width;
            float cellY = sender.InternalResolution.Height / (float)ConsoleSize.Height;
            
            List<float> vertices = new List<float>(4096);
            List<uint> indices = new List<uint>(4096);

            float[] db = ColorToFloatArray(DefaultBackColor);
            float[] df = ColorToFloatArray(DefaultForeColor);

            int vertexCount = 0;

            for (int xi = 0; xi < ConsoleSize.Width; xi++)
            {
                float x = xi * cellX;
                for (int yi = 0; yi < ConsoleSize.Height; yi++)
                {
                    RenderGlyph glyphData = GetGlyph(xi, yi);

                    if (ShowCursor && xi == Cursor.X && yi == Cursor.Y && glyphData == null)
                    {
                        int val = (int)((args.CurrentTime.TotalRuntime / 1000.0f) / BlinkInterval);
                        if (val % 2 == 0 || BlinkInterval == 0)
                            glyphData = new RenderGlyph('_');
                    }

                    float y = yi * cellY;
                    Rectangle texRec = Rectangle.Empty;
                    int glyphId = (int)(glyphData?.Glyph ?? ' ');
                    if (fontAtlas.ContainsId(glyphId))
                        texRec = fontAtlas.GetPointerById(glyphId).Bounds;
                    else if (fontAtlas.ContainsId(0x558))
                        texRec = fontAtlas.GetPointerById(0x558).Bounds;
                    else
                        texRec = fontAtlas.GetPointerById((int)'?').Bounds;

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

            Gl.BindVertexArray(VAO);
            Gl.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            float[] fvertices = vertices.ToArray();
            vertices.Clear();

            uint[] iindices = indices.ToArray();
            indices.Clear();

            CheckGlErrors("pre-vab");
            GCHandle buffAddress = GCHandle.Alloc(fvertices, GCHandleType.Pinned);
            Gl.BufferData(BufferTarget.ArrayBuffer, fvertices.Length * sizeof(float), buffAddress.AddrOfPinnedObject(), BufferUsageHint.StreamDraw);
            buffAddress.Free();

            CheckGlErrors("pre-eab");
            buffAddress = GCHandle.Alloc(iindices, GCHandleType.Pinned);
            Gl.BufferData(BufferTarget.ElementArrayBuffer, iindices.Length * sizeof(float), buffAddress.AddrOfPinnedObject(), BufferUsageHint.StreamDraw);
            buffAddress.Free();

            CheckGlErrors("predraw");
            Gl.DrawElements(BeginMode.Triangles, iindices.Length, DrawElementsType.UnsignedInt, 0);

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
            if (Renderer.TargetWindow.KeyboardState.IsKeyDown(Key.Escape))
            {
                args.AbortExecution = true;
                return;
            }
        }

        private void TargetWindow_KeyDown(OpenToolkit.Windowing.Common.KeyboardKeyEventArgs obj)
        {
            ConsoleRenderProgramKeyEventArgs args = new ConsoleRenderProgramKeyEventArgs(obj);

            OnKeyDown(args);

            if (args.Intercept)
                return;

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

            string value = OpenGL.KeyInput.KeyConverter.Default.KeyToString((int)obj.Key, Renderer.TargetWindow.KeyboardState, Renderer.TargetWindow.KeyboardState.IsKeyDown(Key.CapsLock));

            if (!string.IsNullOrEmpty(value))
                Write(value);
        }

        protected virtual void OnKeyUp(ConsoleRenderProgramKeyEventArgs e)
        {
            KeyUp?.Invoke(this, e);
        }

        protected virtual void OnKeyDown(ConsoleRenderProgramKeyEventArgs e)
        {
            KeyDown?.Invoke(this, e);
        }
    }
}