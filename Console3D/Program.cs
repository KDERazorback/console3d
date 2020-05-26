using com.RazorSoftware.Logging;
using GLFW;
using System;
using System.Collections.Generic;
using System.Drawing;
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

            Log.WriteLine("Initializing OpenGL...");

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

        private static void RenderThread_ProcessingRawInput(OpenGL.RenderThread sender, OpenGL.RenderTimer timer)
        {
            if (Glfw.GetKey(sender.TargetWindow, Keys.Escape) == InputState.Press)
            {
                if (sender.Asynchronous)
                    sender.Stop();
                else
                    AbortMainLoop = true;
            }
        }
    }
}
