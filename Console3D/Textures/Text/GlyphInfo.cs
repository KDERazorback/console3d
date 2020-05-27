using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Console3D.Textures.Text
{
    public struct GlyphInfo
    {
        public string Literal { get; }
        public int CodePoint => char.ConvertToUtf32(Literal, 0);
        public string Description { get; }

        public GlyphInfo(string literal, string description)
        {
            if (literal == null)
                throw new ArgumentNullException("The literal string parameter cannot be null.");
            if (literal.Length > 2)
                throw new ArgumentException("Invalid string literal passed to " + nameof(GlyphInfo) + " constructor.");

            Literal = literal;
            Description = description;
        }
        public GlyphInfo(char ch, string description)
        {
            Literal = new string(ch, 1);
            Description = description;
        }
        public GlyphInfo(int code, string description)
        {
            Literal = char.ConvertFromUtf32(code);
            Description = description;
        }
    }
}
