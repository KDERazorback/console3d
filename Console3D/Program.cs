﻿using com.RazorSoftware.Logging;
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

            Log.WriteLine("Initializing OpenGL...");
            OpenGL.Glfw.LibraryLoadManager.AssemblyLoader.LoadNativeGlfw();

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
            OpenGL.Gl.UnbindApi();

            return 0;
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
                FileInfo targetFile = FontAtlas.GetAtlasFileFromName(cacheDir.FullName, family);
                FileInfo targetFileMetadata = FontAtlas.GetAtlasMetadataFileFromName(cacheDir.FullName, family);

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
    }
}
