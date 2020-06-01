using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Console3D
{
    public class RenderGlyph
    {
        public RenderGlyph(char glyph)
        {
            Glyph = glyph;
        }

        public Color? Foreground { get; set; }
        public Color? Background { get; set; }
        public char Glyph { get; set; }
    }
}
