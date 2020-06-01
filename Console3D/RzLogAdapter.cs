using com.RazorSoftware.Logging;
using com.RazorSoftware.Logging.Outputs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D
{
    public class RzLogAdapter : IOutput
    {
        public RzLogAdapter(ConsoleRenderProgram renderProgram)
        {
            RenderProgram = renderProgram ?? throw new ArgumentNullException(nameof(renderProgram));
        }

        public ConsoleRenderProgram RenderProgram { get; }
        public string Identifier => "console3d";

        public LogLevel MinimumLevel { get; set; }
        public bool Enabled { get; set; } = true;

        public void Close()
        {
            RenderProgram.Stop();
        }

        public void Write(string entry)
        {
            RenderProgram.Write(entry);
        }

        public void Write(string entry, LogLevel severity)
        {
            if (severity >= MinimumLevel)
                RenderProgram.Write(entry);
        }

        public void WriteLine(string entry)
        {
            RenderProgram.WriteLine(entry);
        }

        public void WriteLine(string entry, LogLevel severity)
        {
            if (severity >= MinimumLevel)
                RenderProgram.WriteLine(entry);
        }

        public void WriteLine()
        {
            RenderProgram.WriteLine();
        }
    }
}
