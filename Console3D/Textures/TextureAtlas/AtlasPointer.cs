using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Console3D.Textures.TextureAtlas
{
    public struct AtlasPointer
    {
        public readonly Size Size;
        public readonly Point Offset;
        public readonly long Id;

        public AtlasPointer(long id, Point offset, Size size)
        {
            Id = id;
            Offset = offset;
            Size = size;
        }

        public Rectangle Bounds => new Rectangle(Offset, Size);

        public override string ToString()
        {
            return string.Format("{0};{1},{2};{3},{4}", Id, Offset.X, Offset.Y, Size.Width, Size.Height);
        }
    }
}
