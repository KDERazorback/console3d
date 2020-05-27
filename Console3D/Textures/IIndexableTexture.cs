using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Console3D.Textures
{
    public interface IIndexableTexture
    {
        public long Id { get; }
        public Bitmap Bitmap { get; }
        public Size Size { get; }
        public Rectangle Bounds { get; }
        public Point Offset { get; }
    }
}
