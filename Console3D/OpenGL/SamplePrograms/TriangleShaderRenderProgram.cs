using com.RazorSoftware.Logging;
using Console3D.OpenGL.Shaders;
using System;
using System.IO;
#if !EMBEDDED_GL
using OpenToolkit.Graphics.OpenGL;
using Gl = OpenToolkit.Graphics.OpenGL.GL;
#endif

namespace Console3D.OpenGL
{
    public class TriangleShaderRenderProgram : RenderProgram
    {
        float[] vertices;
        uint[] indices;
        ShaderProgram shader;

        int VAO, VBO, EBO;

        public TriangleShaderRenderProgram(RenderThread renderer) : base(renderer) 
        {
            Renderer.AutoClearFrames = false;
            Renderer.AutoEnableCaps = AutoEnableCapabilitiesFlags.None;
            Renderer.AutoSetViewport = false;
        }

        protected override void Renderer_ContextCreated(RenderThread sender, ContextCreationEventArgs args)
        {
            Log.WriteLine("Context created. Using: " + Gl.GetString(StringName.Renderer) + Gl.GetString(StringName.Version));

            Log.WriteLine("Renderer_ContextCreated");
            CheckGlErrors("Renderer_ContextCreated:Start");

            // Set window resize callback
            //GLFW.Glfw.SetFramebufferSizeCallback(Renderer.TargetWindow, Framebuffer_SizeCallback);

            // Load Shaders
            string vertex = File.ReadAllText("./shaders/hello.vert".Replace('/', Path.DirectorySeparatorChar));
            string fragment = File.ReadAllText("./shaders/hello.frag".Replace('/', Path.DirectorySeparatorChar));

            // Compile shaders
            Log.WriteLine("Compiling shader 'hello'...");
            shader = new Shaders.ShaderProgram(vertex, fragment);
            int shaderId = shader.Compile();
            Log.WriteLine("Compiled shader index: %@", LogLevel.Message, shader.ProgramId);

            CheckGlErrors("Renderer_ContextCreated:AfterShaders");


            // Load vertics and indices
            vertices = new float[] {
                 0.5f,  0.5f, 0.0f,  // top right
                 0.5f, -0.5f, 0.0f,  // bottom right
                -0.5f, -0.5f, 0.0f,  // bottom left
                -0.5f,  0.5f, 0.0f   // top left 
            };
            indices = new uint[] {  // note that we start from 0!
                0, 1, 3,  // first Triangle
                1, 2, 3   // second Triangle
            };

            // Setup global VAO
            VAO = Gl.GenVertexArray();
            Gl.BindVertexArray(VAO);

            CheckGlErrors("Renderer_ContextCreated:AfterVAO");


            // Setup VBO (vertices)
            VBO = Gl.GenBuffer();
            Gl.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            int size = (sizeof(float) * vertices.Length); // size must be 48
            Gl.BufferData(BufferTarget.ArrayBuffer, size, vertices, BufferUsageHint.StaticDraw);
            CheckGlErrors("Renderer_ContextCreated:AfterBufferDataVBO");

            Gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            CheckGlErrors("Renderer_ContextCreated:AfterVAP_VBO");

            Gl.EnableVertexAttribArray(0);

            CheckGlErrors("Renderer_ContextCreated:AfterVBO");


            // Setup EBO (indices)
            EBO = Gl.GenBuffer();
            Gl.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            size = (sizeof(float) * indices.Length); // Must be 24
            Gl.BufferData(BufferTarget.ElementArrayBuffer, size, indices, BufferUsageHint.StaticDraw);

            CheckGlErrors("Renderer_ContextCreated:AfterEBO");


            // Unbind
            Gl.BindVertexArray(0);

            CheckGlErrors("Renderer_ContextCreated:End");
        }
        protected override void Renderer_Draw(RenderThread sender, FrameStageEventArgs args)
        {
            Log.WriteLine("Renderer_Draw(%@)", LogLevel.Message, args.CurrentTime.FrameIndex);

            Gl.BindVertexArray(VAO);
            CheckGlErrors("Renderer_Draw:BindVAO");


            Gl.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            CheckGlErrors("Renderer_Draw:Clear");


            Gl.UseProgram(shader.ProgramId);
            CheckGlErrors("Renderer_Draw:UseProgram");


            Gl.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
            CheckGlErrors("Renderer_Draw:DrawElements");

        }

        protected void Framebuffer_SizeCallback(IntPtr window, int width, int height)
        {
            Gl.Viewport(0, 0, width, height);
        }
    }
}
