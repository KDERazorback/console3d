using Console3D.Textures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Console3D.Textures.Text
{
    public struct Glyph : IIndexableTexture
    {
        public long Id => Metadata.CodePoint;
        public Size Size => Bitmap.Size;
        public Point Offset => Point.Empty;
        public Rectangle Bounds => new Rectangle(Offset, Size);

        public Glyph(GlyphInfo metadata, Bitmap bitmap)
        {
            Metadata = metadata;
            Bitmap = bitmap ?? throw new ArgumentNullException(nameof(bitmap));
        }

        public GlyphInfo Metadata { get; }
        public Bitmap Bitmap { get; }
    }
}
