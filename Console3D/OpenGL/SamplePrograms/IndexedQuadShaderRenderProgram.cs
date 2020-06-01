using com.RazorSoftware.Logging;
using Console3D.OpenGL.Shaders;
using OpenToolkit.Graphics.OpenGL;
using System.IO;
using Gl = OpenToolkit.Graphics.OpenGL.GL;

namespace Console3D.OpenGL.SamplePrograms
{
    public class IndexedQuadShaderRenderProgram : RenderProgram
    {
        private uint[] indices;
        private ShaderProgram shader;
        private int VAO, VBO, EBO;
        private float[] vertices;
        public IndexedQuadShaderRenderProgram(RenderThread renderer) : base(renderer)
        {
            Renderer.AutoClearFrames = false;
            Renderer.AutoEnableCaps = AutoEnableCapabilitiesFlags.None;
            Renderer.WireframeMode = true;
        }

        protected override void Renderer_ContextCreated(RenderThread sender, ContextCreationEventArgs args)
        {
            Log.WriteLine("Context created. Using: " + Gl.GetString(StringName.Renderer) + Gl.GetString(StringName.Version));

            Log.WriteLine("Renderer_ContextCreated");

            Renderer.TargetWindow.Resize += TargetWindow_Resize;

            // Load Shaders
            string vertex = File.ReadAllText("./shaders/hello.vert".Replace('/', Path.DirectorySeparatorChar));
            string fragment = File.ReadAllText("./shaders/hello.frag".Replace('/', Path.DirectorySeparatorChar));

            // Compile shaders
            Log.WriteLine("Compiling shader 'hello'...");
            shader = new ShaderProgram(vertex, fragment);
            shader.Compile();
            Log.WriteLine("Compiled shader index: %@", LogLevel.Message, shader.ProgramId);

            // Load vertices and indices
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

        private void TargetWindow_Resize(OpenToolkit.Windowing.Common.ResizeEventArgs obj)
        {
            Gl.Viewport(0, 0, obj.Width, obj.Height);
        }
    }
}