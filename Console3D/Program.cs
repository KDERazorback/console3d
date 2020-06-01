using com.RazorSoftware.Logging;
using Console3D.OpenGL;
using Console3D.Textures.Text;
using Console3D.Textures.TextureAtlas;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Console3D
{
    public static class Program
    {
        private static bool restartGraphicsProgram = false;

        public static int Main(string[] args)
        {
            Log.Initialize(true);
            Log.Console.Enabled = true;
            Log.Console.MinimumLevel = LogLevel.Debug;

            Log.WriteLine("Console3D Test App");
            Log.WriteLine("Running on %@ %@ (%@)",
                LogLevel.Message,
                GetPlatformName(),
                RuntimeInformation.ProcessArchitecture,
                RuntimeInformation.OSDescription);
            Log.WriteLine();

            Log.WriteLine("Checking custom fonts...");
            CheckRasterFonts();

            Log.WriteLine("Starting Render Thread...");

            OpenGL.RenderThread renderThread = new RenderThread(new Size(960, 480), new Size(960, 480));
            renderThread.Asynchronous = false;
            renderThread.WindowTitle = "Console3D - OpenGL";
            renderThread.Initialize();

            ConsoleRenderProgram program;
            program = new ConsoleRenderProgram(renderThread);
            program.FontName = "Unifont";
            program.KeyUp += Program_KeyUp;

            Log.WriteLine("Starting render thread in %@ mode...",
                LogLevel.Message,
                renderThread.Asynchronous ? "Asynchronous" : "Synchronous");

            RzLogAdapter adapter = new RzLogAdapter(program);
            Log.AttachOutput(adapter);

            program.Run();

            // Main thread is free

            while (restartGraphicsProgram && !renderThread.Asynchronous)
            {
                restartGraphicsProgram = false;
                program.Stop();
                program.Run();
            }
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

            return 0;
        }

        private static void Program_KeyUp(ConsoleRenderProgram sender, ConsoleRenderProgramKeyEventArgs e)
        {
            // DEBUG: Print key to the general log
            char c = (char)(e.Key - 19);
            string cval = ((int)e.Key).ToString("X2");
            Log.WriteLine($"0x{cval} {e.Key} -> {c}");

            e.Intercept = true;

            if (e.Key == OpenToolkit.Windowing.Common.Input.Key.Escape)
                sender.Stop();

            if (e.Key == OpenToolkit.Windowing.Common.Input.Key.F11)
            {
                restartGraphicsProgram = true;
                sender.Stop();
                sender.Renderer.Fullscreen = !sender.Renderer.Fullscreen;

                if (sender.Renderer.Fullscreen)
                {
                    sender.Renderer.InternalResolution = new Size(1280, 1024);
                    sender.ConsoleSize = new Size(300, 75);
                }
                else
                {
                    sender.Renderer.InternalResolution = new Size(960, 480);
                    sender.ConsoleSize = new Size(120, 30);
                }
            }

            if (e.Key == OpenToolkit.Windowing.Common.Input.Key.F1)
            {
                sender.ForeColor = Color.Cyan;
                Log.WriteLine("Colored line sample here");
                sender.ForeColor = sender.DefaultForeColor;
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

                    Atlas fontAtlas = AtlasBuilder.BuildAtlas(rasterizedFont, AtlasLayoutMode.Indexed);
                    fontAtlas.ToFile(targetFile.FullName, targetFileMetadata.FullName, ImageFormat.Bmp);
                }
            }
        }

        private static string GetPlatformName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return "windows";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return "linux";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return "macosx";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
                return "freebsd";

            return "unknown";
        }
    }
}