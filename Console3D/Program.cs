using com.RazorSoftware.Logging;
using Console3D.Textures.Text;
using Console3D.Textures.TextureAtlas;
using GLFW;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Text;
using System.Threading;

namespace Console3D
{
    public static class Program
    {
        private static bool AbortMainLoop = false;
        private static bool ForceLocalLibraries = false;
        private static OpenGL.Shaders.ShaderProgram glyphShader;
        private static Size ConsoleSize = new Size(120, 30);
        private static Textures.TextureAtlas.Atlas fontAtlas;
        private static uint fontAtlasTexture;

        public static int Main(string[] args)
        {
            Log.Initialize(true);
            Log.Console.Enabled = true;
            Log.Console.MinimumLevel = LogLevel.Debug;

            OpenGL.Glfw.LibraryLoadManager.Initialize();

            Log.WriteLine("Console3D Test App");
            Log.WriteLine("Running on %@ %@ (%@)",
                LogLevel.Message,
                OpenGL.Glfw.LibraryLoadManager.AssemblyLoader.PlatformInfo.DetectedPlatformName,
                System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture,
                System.Runtime.InteropServices.RuntimeInformation.OSDescription);
            Log.WriteLine();

            Log.WriteLine("Checking custom fonts...");
            CheckRasterFonts();
            fontAtlas = Textures.TextureAtlas.Atlas.FromFile(
                FontAtlas.GetAtlasFileFromName("./cache/fonts", "Code New Roman").FullName,
                FontAtlas.GetAtlasMetadataFileFromName("./cache/fonts", "Code New Roman").FullName
            );

            Log.WriteLine("Initializing OpenGL...");
            if (ForceLocalLibraries)
            {
                Log.WriteLine("Forcing usage of local OpenGL libraries installed on the application directory.");
                OpenGL.Glfw.LibraryLoadManager.AssemblyLoader.LoadNativeGlfw();
            }

            Glfw.Init();
            Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.OpenglForwardCompatible, true);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
            Glfw.WindowHint(Hint.Doublebuffer, true);
            Glfw.WindowHint(Hint.Decorated, true);

            Log.WriteLine("Starting Render Thread...");

            OpenGL.RenderThread renderThread = new OpenGL.RenderThread(new Size(800, 600), new Size(800, 600));
            renderThread.Asynchronous = false;
            renderThread.WindowTitle = "Console3D - OpenGL";
            renderThread.Initialize();
            if (!string.Equals(OpenGL.Glfw.LibraryLoadManager.AssemblyLoader.PlatformInfo.DetectedPlatformName, "windows", StringComparison.OrdinalIgnoreCase))
            {
                renderThread.AutoEventPolling = false;
                renderThread.CreateMainWindow();
            }

            renderThread.ProcessingRawInput += RenderThread_ProcessingRawInput;
            renderThread.DrawPrepare += RenderThread_DrawPrepare;
            renderThread.Draw += RenderThread_Draw;

#if EMBEDDED_GL
            if (!OpenGL.Gl.IsApiBound)
                OpenGL.Gl.BindApi();
#else
            global::OpenGL.Gl.BindAPI();
#endif

            Log.WriteLine("Compiling shaders...");
            Log.WriteLine("Compiling 'glyph' program...");
            glyphShader = OpenGL.Shaders.ShaderProgram.FromFiles("./shaders/glyph.vert", "./shaders/glyph.frag");
            glyphShader.Compile();
            Log.WriteLine("Shader compilation completed.");

            Log.WriteLine("Loading textures...");
            Log.WriteLine("Loading font atlas 'Code New Roman'...");
            fontAtlasTexture = LoadTexture(fontAtlas.Data, global::OpenGL.TextureWrapMode.ClampToBorder);

            Log.WriteLine("Starting render thread in %@ mode...",
                LogLevel.Message,
                renderThread.Asynchronous ? "Asynchronous" : "Synchronous");

            renderThread.Start();

            if (!renderThread.Asynchronous)
            {
                while (!Console.KeyAvailable && !AbortMainLoop)
                {
                    renderThread.ProcessEvents();
                    renderThread.AdvanceFrame();
                }
            }

            // Main thread is free
            while (renderThread.Asynchronous && !Console.KeyAvailable && renderThread.IsRunning)
            {
                if (!renderThread.AutoEventPolling)
                {
                    renderThread.ProcessEvents();
                    if (renderThread.TimeSinceLastFrame > 1000)
                    {
                        int amount = (int)(renderThread.TimeSinceLastFrame / 2000.0f);
                        if (amount >= 3)
                            Thread.Sleep(amount);
                    }
                }
                else
                    Thread.Sleep(1000);
            }

            renderThread.Stop();
            Log.WriteLine("Render thread has been stopped.");

            Log.WriteLine("Shutting down Render thread...");
            renderThread.Shutdown();

            Log.WriteLine("Cleaning up...");
            renderThread.Dispose();
            OpenGL.Glfw.LibraryLoadManager.Shutdown();
#if EMBEDDED_GL
            OpenGL.Gl.UnbindApi();
#endif

            return 0;
        }

        private static void RenderThread_Draw(OpenGL.RenderThread sender, OpenGL.FrameStageEventArgs args)
        {
            if (!sender.IsMainThread() && !sender.Asynchronous)
                throw new InvalidOperationException("Draw operation initiated from secondary thread when supposed to run in Sync mode.");

            int cellX = sender.InternalResolution.Width / ConsoleSize.Width;
            int cellY = sender.InternalResolution.Height / ConsoleSize.Height;

            uint vao = global::OpenGL.Gl.GenVertexArray();
            global::OpenGL.Gl.BindVertexArray(vao);

            CheckGlErrors("pre-texture");

            sender.SetUniform("resolution", 0.0f, 0.0f, sender.InternalResolution.Width, sender.InternalResolution.Height);
            global::OpenGL.Gl.ActiveTexture(global::OpenGL.TextureUnit.Texture0);
            global::OpenGL.Gl.BindTexture(global::OpenGL.TextureTarget.Texture2d, fontAtlasTexture);

            CheckGlErrors("post-texture");

            List<float> vertices = new List<float>(4096);
            List<uint> indices = new List<uint>(4096);

            for (int x = 0; x < cellX; x++)
            {
                for (int y = 0; y < cellY; y++)
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

            uint vbo = global::OpenGL.Gl.GenBuffer();
            global::OpenGL.Gl.BindBuffer(global::OpenGL.BufferTarget.ArrayBuffer, vbo);

            CheckGlErrors("pre-vab");

            float[] fvertices = vertices.ToArray();
            global::OpenGL.Gl.BufferData(global::OpenGL.BufferTarget.ArrayBuffer, (uint)fvertices.Length * sizeof(float), fvertices, global::OpenGL.BufferUsage.StreamDraw);

            CheckGlErrors("pre-eab");

            uint[] iindices = indices.ToArray();
            uint ebo = global::OpenGL.Gl.GenBuffer();
            global::OpenGL.Gl.BindBuffer(global::OpenGL.BufferTarget.ElementArrayBuffer, ebo);
            global::OpenGL.Gl.BufferData(global::OpenGL.BufferTarget.ElementArrayBuffer, (uint)iindices.Length * sizeof(int), iindices, global::OpenGL.BufferUsage.StreamDraw);

            CheckGlErrors("pre_va_pointer");

            global::OpenGL.Gl.VertexAttribPointer(0, 2, global::OpenGL.VertexAttribType.Float, false, 12 * sizeof(float), 0 * sizeof(float));
            CheckGlErrors("post-va-pointer-0-set");
            global::OpenGL.Gl.EnableVertexAttribArray(0);
            CheckGlErrors("post-va-pointer-0-enable");

            global::OpenGL.Gl.VertexAttribPointer(1, 2, global::OpenGL.VertexAttribType.Float, false, 12 * sizeof(float), 2 * sizeof(float));
            global::OpenGL.Gl.EnableVertexAttribArray(1);
            CheckGlErrors("post-va-pointer-1");

            global::OpenGL.Gl.VertexAttribPointer(2, 4, global::OpenGL.VertexAttribType.Float, false, 12 * sizeof(float), 4 * sizeof(float));
            global::OpenGL.Gl.EnableVertexAttribArray(2);
            CheckGlErrors("post-va-pointer-2");

            global::OpenGL.Gl.VertexAttribPointer(3, 4, global::OpenGL.VertexAttribType.Float, false, 12 * sizeof(float), 8 * sizeof(float));
            global::OpenGL.Gl.EnableVertexAttribArray(3);

            CheckGlErrors("predraw");

            global::OpenGL.Gl.DrawElements(global::OpenGL.PrimitiveType.TriangleFan, 5, global::OpenGL.DrawElementsType.UnsignedInt, 0);

            CheckGlErrors("final");
            global::OpenGL.Gl.BindVertexArray(0);
        }

        private static void RenderThread_DrawPrepare(OpenGL.RenderThread sender, OpenGL.FrameStageControllerEventArgs args)
        {
            CheckGlErrors("pre-program");
            sender.SelectShader(glyphShader);
            CheckGlErrors("post-program");
        }

        private static void RenderThread_ProcessingRawInput(OpenGL.RenderThread sender, OpenGL.FrameStageEventArgs args)
        {
            if (Glfw.GetKey(sender.TargetWindow, Keys.Escape) == InputState.Press)
            {
                if (sender.Asynchronous)
                    sender.Stop();
                else
                    AbortMainLoop = true;
            }
        }

        private static void CheckRasterFonts(string fontsDirectory = "./fonts", string rasterDirectory = "./cache/fonts")
        {
            DirectoryInfo cacheDir = new DirectoryInfo(rasterDirectory.Replace('/', Path.DirectorySeparatorChar));
            if (!cacheDir.Exists)
                cacheDir.Create();

            DirectoryInfo fontsDir = new DirectoryInfo(fontsDirectory.Replace('/', Path.DirectorySeparatorChar));
            if (!fontsDir.Exists)
            {
                fontsDir.Create();
                throw new FileNotFoundException(string.Format("The fonts directory doesnt exists ({0}). No font files installed on the application.", fontsDirectory));
            }

            FileInfo[] files = fontsDir.GetFiles();
            long fontCount = 0;

            if (files != null)
            {
                foreach (FileInfo font in files)
                {
                    if (string.Equals(font.Extension, ".otf", StringComparison.OrdinalIgnoreCase) || string.Equals(font.Extension, ".ttf", StringComparison.OrdinalIgnoreCase))
                    { 
                        Log.WriteLine("Loading font file %@...", LogLevel.Message, font.Name);
                        FontLoader.LoadFromFile(font.FullName);
                        fontCount++;
                    }
                }
            }

            if (files == null || files.Length < 1 || fontCount < 1)
                throw new FileNotFoundException(string.Format("No OTF font files found on the fonts directory ({0}). No font files installed on the application.", fontsDirectory));

            Log.WriteLine("Loaded %@ fonts into the application.", LogLevel.Message, FontLoader.LoadedFonts.Length.ToString("N0"));

            GdiFontRasterizer rasterizer = new GdiFontRasterizer(true, true, true);
            foreach (FontFamily family in FontLoader.LoadedFonts)
            {
                FileInfo targetFile = FontAtlas.GetAtlasFileFromName(cacheDir.FullName, family.Name);
                FileInfo targetFileMetadata = FontAtlas.GetAtlasMetadataFileFromName(cacheDir.FullName, family.Name);

                if (!targetFile.Exists)
                {
                    // Build font
                    Log.WriteLine("Font '%@' is missing from cache. Rasterizing it...", LogLevel.Message, family.Name);
                    
                    rasterizer.SelectedFont = new Font(family, 18.0f);
                    GlyphCollection rasterizedFont = rasterizer.Raster();

                    if (rasterizedFont.Count < 1)
                    {
                        Log.WriteLine("Failed to process font file %@. No glyphs found inside the font file.", LogLevel.Error, family.Name);
                        continue; ;
                    }

                    Atlas fontAtlas = AtlasBuilder.BuildAtlas(rasterizedFont, AtlasLayoutMode.Positional);
                    fontAtlas.ToFile(targetFile.FullName, targetFileMetadata.FullName, ImageFormat.Bmp);
                }
            }
        }

        private static uint LoadTexture(Bitmap data, global::OpenGL.TextureWrapMode mode)
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

            // TODO: Add embedded GL code here
#if EMBEDDED_GL
            uint textureId = Gl.GenTexture();
            Gl.BindTexture(global::OpenGL.TextureTarget.Texture2d, textureId);
            throw new NotImplementedException();
#else
            uint textureId = global::OpenGL.Gl.GenTexture();
            global::OpenGL.Gl.BindTexture(global::OpenGL.TextureTarget.Texture2d, textureId);
            global::OpenGL.Gl.TexParameter(global::OpenGL.TextureTarget.Texture2d, global::OpenGL.TextureParameterName.TextureWrapS, (int)mode);
            global::OpenGL.Gl.TexParameter(global::OpenGL.TextureTarget.Texture2d, global::OpenGL.TextureParameterName.TextureWrapT, (int)mode);
            global::OpenGL.Gl.TexImage2D(global::OpenGL.TextureTarget.Texture2d,
                0,
                global::OpenGL.InternalFormat.Rgba8,
                data.Width, data.Height,
                0,
                global::OpenGL.PixelFormat.Rgba,
                global::OpenGL.PixelType.UnsignedByte,
                buffer);
#endif


            if (mode == global::OpenGL.TextureWrapMode.ClampToBorder)
            {
                float[] borderColor = new float[] { 0xff, 0x14, 0x93 };
                global::OpenGL.Gl.TexParameter(global::OpenGL.TextureTarget.Texture2d, global::OpenGL.TextureParameterName.TextureBorderColor, borderColor);
            }

            return textureId;
        }

        private static void CheckGlErrors(string stage)
        {
            global::OpenGL.ErrorCode code = global::OpenGL.Gl.GetError();

            if (code != global::OpenGL.ErrorCode.NoError)
                throw new System.Exception(string.Format("OpenGL Draw error on stage {0}: {1}", stage, code.ToString()));
        }
    }
}
