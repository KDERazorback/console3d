# Console3D - A .NET 3D Console implementation

This library is an implementation of a customizable Text-based Console using OpenGL 3D outputs, written in .NET Core 3.1 with portability and cross-platform support in mind.

# General Requirements
- A system with driver and hardware support for OpenGL version 3.3 or later.
- Microsoft GDI+ Library support installed on the system, for Font Rasterization.
- GLFW v3.0 or later installed on the system.

## Windows
- GDI+ is already included in Windows XP or later.
- GLFW3 is already included as part of the library resources.

## Linux
1. Install `glfw3` or equivalent from your package manager.
2. Install `libgdiplus` from your package manager. (_Optional: for font rasterization support_).

## MacOS
1. Install brew [from here](https://brew.sh)
2. Install glfw with:

    `brew update; brew install glfw`

# Important Threading limitations
On Linux and MacOS, all calls related to Window management (event polling, UI and Viewport redraw, input processing, etc.) must happen inside the process's Main Thread due to restrictions imposed by the OS itself. Due to this, when running on those platforms, the `Asynchronous` option of the library must be disabled, and the main rendering loop must be called from within the Application's main thread. Its recommended to wrap your application's entry point into a separate method, and call it from within another thread if your application dont rely on any other form of UI. Otherwise, you may need to write your own window-management loop on the main thread, and manually poll for window events while your application is running. On Windows, this restriction is not present and you can fork a new background Thread for Window Management and Rendering.

# Examples and Usage
To use the library, an small amount of boilerplate code is required to create and initialize the rendering backend.

You can choose to let the Console3D library automatically handle the rendering loop, or manually perform each iteration yourself.

## Common helper methods:
```
static ConsoleRenderProgram InitConsole3D(string title, Size windowRes, Size internalRes)
    {
        RenderThread renderer = new RenderThread(windowRes, internalRes);
        renderer.Asynchronous = false;
        renderer.Title = title;
        renderer.Initialize();

        ConsoleRenderProgram program = new ConsoleRenderProgram(renderer);
        program.FontName = "Unifont";

        return program;
    }
```

## Option 1: Manually perform each rendering iteration:
```
    static void main()
    {
        Size resolution = new Size(940, 480);
        ConsoleRenderProgram console = InitConsole3D("My Console3D", resolution, resolution);

        console.Renderer.Start();
        

        // Continue with your app initialization here

        while (...)
        {
            console.Renderer.ProcessEvents();
            ...
            ...
            // Your app code here (iteration-based)
            console.WriteLine("Hello World!"); // Example: Write some text to the console
            ...
            ...
            console.Renderer.AdvanceFrame();
        }

        // Shutdown
    }
```

## Option 2: Let the library handle the rending loop:

```
    static ConsoleRenderProgram Console;

    static void main()
    {
        Size resolution = new Size(940, 480);
        Console = InitConsole3D("My Console3D", resolution, resolution);

        Console.SyncRenderStart += Console_OnRenderStart;

        // Continue with your app initialization here

        Console.Run(); // Blocking-call until an exit is requested

        // Shutdown
    }

    static void Console_OnRenderStart(RenderProgram sender, RenderProgramEventArgs args)
    {
        // Your app code here (iteration-based)
        sender.WriteLine("Hello World!"); // Example: Write to the active console


        if (...some-quit-condition...)
        {
            args.AbortExecution = true; // Stop rendering loop
            return;
        }
    }
```

## Option 3: Run the app on a background thread
```
    static ConsoleRenderProgram Console;

    static void main()
    {
        Size resolution = new Size(940, 480);
        Console = InitConsole3D("My Console3D", resolution, resolution);

        Thread appThread = new Thread(AppMain);
        appThread.IsBackground = true;
        
        appThread.Start(); // Start your app from the intended entry point

        Console.Run(); // Start the console rendering loop synchronously on the main thread

        // Shutdown
    }

    static void AppMain()
    {
        // Your code here.

        Console.WriteLine("Hello world!"); // Example: Send some data to the console across threads.
    }
```

## Option 4: Asynchronous rendering loop (Windows only)
```
    static void main()
    {
        Size resolution = new Size(940, 480);
        RenderThread renderer = new RenderThread(resolution, resolution);
        renderer.Asynchronous = true; // Windows-only
        renderer.Title = "My Console";
        renderer.Initialize();

        ConsoleRenderProgram program = new ConsoleRenderProgram(renderer);
        program.FontName = "Unifont";

        program.Run(); // Asynchronous non-blocking call

        // Continue with app initialization here

        // Your app code
        program.WriteLine("Hello World!");

        // Shutdown
    }
```

# Font rasterization
In order to draw something on the 3D Console you need to provide a valid font atlas that contains pre-rendered glyphs for each character you want to print. Font Atlas generation is achieved by loading the Font file, and processing each character for a corresponding character-set, and drawing them onto a canvas at an specified position. The final image is then loaded onto the GPU as a texture, and used to draw each character printed to it.

Font Rasterization needs to be done only once per combination of _Font Family_ and _Font Style_ to be used. After that, the atlas can be used multiple times, even copied across different platforms or devices.

The library includes utility classes used to perform font rasterization from multiple source font files. These utility classes uses **GDI+** for this task.

## Load and Rasterize a Font from an specified filename
```
    FontLoader.LoadFromFile("fonts/Unifont.otf");
    GdiFontRasterizer rasterizer = new GdiFontRasterizer(true, true, true); // Raster all available glyphs

    string atlasFilename = FontAtlas.GetAtlasFileFromName("./cache/fonts", FontName).FullName;
    string atlasMetadata = FontAtlas.GetAtlasMetadataFileFromName("./cache/fonts", FontName).FullName;

    foreach (FontFamily family in FontLoader.LoadedFonts)
    {
        rasterizer.SelectedFont = new Font(family, 18.0f); // FontSize of 18.0 points
        GlyphCollection rasterizedFont = rasterizer.Raster();

        Atlas fontAtlas = AtlasBuilder.BuildAtlas(rasterizedFont, AtlasLayoutMode.Indexed);

        fontAtlas.ToFile(atlasFilename, atlasMetadata, ImageFormat.Bmp); // Rasterize it
    }
```

_Note that Altas files are automatically loaded by the `ConsoleRenderProgram` instance upon initialization._