using com.RazorSoftware.Logging;
using Console3D.OpenGL;
using Console3D.Textures.Text;
using Console3D.Textures.TextureAtlas;
using GLFW;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;

namespace Console3D
{
    public static class Program
    {
        private static bool ForceLocalLibraries = false;

        public static int Main(string[] args)
        {
            Environment.SetEnvironmentVariable("OPENGL_NET_EGL_STATIC_INIT", "NO");

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

            //Gl.Initialize();

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

            RenderProgram program;
            //program = new ConsoleRenderProgram(renderThread);
            program = new TriangleRenderProgram(renderThread);

            Log.WriteLine("Starting render thread in %@ mode...",
                LogLevel.Message,
                renderThread.Asynchronous ? "Asynchronous" : "Synchronous");

            program.Run();

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

            program.Stop();
            Log.WriteLine("Render thread has been stopped.");

            Log.WriteLine("Shutting down Render thread...");
            program.Dispose();

            OpenGL.Glfw.LibraryLoadManager.Shutdown();
#if EMBEDDED_GL
            OpenGL.Gl.UnbindApi();
#endif

            return 0;
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

            GdiFontRasterizer rasterizer = new GdiFontRasterizer(true, false, false);
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

                    Atlas fontAtlas = AtlasBuilder.BuildAtlas(rasterizedFont, AtlasLayoutMode.Indexed);
                    fontAtlas.ToFile(targetFile.FullName, targetFileMetadata.FullName, ImageFormat.Bmp);
                }
            }
        }
    }
}
