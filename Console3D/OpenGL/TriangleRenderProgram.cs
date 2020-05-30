using com.RazorSoftware.Logging;
using Console3D.OpenGL.Shaders;
using System;
using System.IO;

namespace Console3D.OpenGL
{
    public class TriangleRenderProgram : RenderProgram
    {
        float[] vertices;
        uint[] indices;
        ShaderProgram shader;

        uint VAO, VBO, EBO;

        public TriangleRenderProgram(RenderThread renderer) : base(renderer) 
        {
            Renderer.AutoClearFrames = false;
            Renderer.AutoSetViewport = false;
            Renderer.AutoEnableCaps = AutoEnableCapabilitiesFlags.None;
        }

        protected override void Renderer_ContextCreated(RenderThread sender, ContextCreationEventArgs args)
        {
            GLFW.Glfw.SetFramebufferSizeCallback(Renderer.TargetWindow, Framebuffer_SizeCallback);

            string vertex = File.ReadAllText("./shaders/hello.vert".Replace('/', Path.DirectorySeparatorChar));
            string fragment = File.ReadAllText("./shaders/hello.frag".Replace('/', Path.DirectorySeparatorChar));

            Log.WriteLine("Compiling shader 'hello'...");
            shader = new Shaders.ShaderProgram(vertex, fragment);
            uint shaderId = shader.Compile();
            Log.WriteLine("Compiled shader index: %@", LogLevel.Message, shader.ProgramId);

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

            VAO = Gl.GenVertexArray();
            VBO = Gl.GenBuffer();
            EBO = Gl.GenBuffer();

            // bind the Vertex Array Object first, then bind and set vertex buffer(s), and then configure vertex attributes(s).
            Gl.BindVertexArray(VAO);

            Gl.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            uint size = (uint)(sizeof(float) * vertices.Length); // size must be 48
            Gl.BufferData(BufferTarget.ArrayBuffer, size, vertices, BufferUsage.StaticDraw);

            Gl.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            size = (uint)(sizeof(float) * indices.Length); // Must be 24
            Gl.BufferData(BufferTarget.ElementArrayBuffer, size, indices, BufferUsage.StaticDraw);

            Gl.VertexAttribPointer(0, 3, VertexAttribType.Float, false, 3 * sizeof(float), 0);
            Gl.EnableVertexAttribArray(0);

            // Unbind
            Gl.BindBuffer(BufferTarget.ArrayBuffer, 0);
            Gl.BindVertexArray(0);
        }
        protected override void Renderer_Draw(RenderThread sender, FrameStageEventArgs args)
        {
            Gl.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            Gl.Clear(ClearBufferMask.ColorBufferBit);

            Gl.UseProgram(shader.ProgramId);

            Gl.BindVertexArray(VAO);

            Gl.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }

        protected void Framebuffer_SizeCallback(IntPtr window, int width, int height)
        {
            Gl.Viewport(0, 0, width, height);
        }
    }
}
