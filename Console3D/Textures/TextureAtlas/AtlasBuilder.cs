using Console3D.Textures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Console3D.Textures.TextureAtlas
{
    public static class AtlasBuilder
    {
        public static Atlas BuildAtlas(IEnumerable<IIndexableTexture> textures, AtlasLayoutMode mode)
        {
            switch (mode)
            {
                case AtlasLayoutMode.Positional:
                    return BuildPositionalAtlas(textures);
                case AtlasLayoutMode.Indexed:
                    return BuildIndexedAtlas(textures);
            }

            throw new ArgumentOutOfRangeException("Unrecognized Atlas mode.");
        }

        private static Atlas BuildIndexedAtlas(IEnumerable<IIndexableTexture> textures)
        {
            ScanTextureCollection(textures, out int textureW, out int textureH, out int textureCount, out int texturesPerRow, out int rows, out long maxTextureId);

            List<AtlasPointer> pointers = new List<AtlasPointer>(textureCount);

            Bitmap buffer = new Bitmap(textureW * texturesPerRow, textureH * rows, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Graphics device = Graphics.FromImage(buffer);
            device.Clear(Color.White);
            device.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            int i = 0;
            foreach (IIndexableTexture tex in textures)
            {
                if (i >= textureCount)
                    break;

                int y = i / texturesPerRow;
                int x = i % texturesPerRow;

                int xpos = x * textureW;
                int ypos = y * textureH;
                xpos += (int)(textureW / 2.0f);
                ypos += (int)(textureH / 2.0f);
                xpos -= (int)(tex.Size.Width / 2.0f);
                ypos -= (int)(tex.Size.Height / 2.0f);

                pointers.Add(new AtlasPointer(tex.Id, new Point(xpos, ypos), tex.Size));

                device.DrawImage(tex.Bitmap, new Rectangle(xpos, ypos, tex.Size.Width, tex.Size.Height), new Rectangle(Point.Empty, tex.Size), GraphicsUnit.Pixel);

                i++;
            }

            device.Flush();
            device.Dispose();

            return new Atlas(buffer, AtlasLayoutMode.Indexed, pointers.ToArray());
        }

        private static void ScanTextureCollection(IEnumerable<IIndexableTexture> textures, out int textureW, out int textureH, out int textureCount, out int texturesPerRow, out int rows, out long maxTextureId)
        {
            textureW = 0;
            textureH = 0;
            textureCount = 0;
            maxTextureId = 0;

            foreach (IIndexableTexture tex in textures)
            {
                if (tex.Size.Width > textureW) textureW = tex.Size.Width;
                if (tex.Size.Height > textureH) textureH = tex.Size.Height;
                if (tex.Id > maxTextureId) maxTextureId = tex.Id;
                textureCount++;
            }

            if (textureCount > 2)
            {
                double x = Math.Sqrt(textureCount);
                if (x > (int)x)
                    texturesPerRow = (int)x + 1;
                else
                    texturesPerRow = (int)x;

                x = textureCount / (double)texturesPerRow;
                if (x > (int)x)
                    rows = (int)x + 1;
                else
                    rows = (int)x;
            }
            else
            {
                texturesPerRow = 2;
                rows = 1;
            }

            if (texturesPerRow * rows < textureCount)
                throw new ApplicationException("Failed to compute atlas size from texture count. Internal error.");
        }

        private static Atlas BuildPositionalAtlas(IEnumerable<IIndexableTexture> textures)
        {
            ScanTextureCollection(textures, out int textureW, out int textureH, out int textureCount, out int texturesPerRow, out int rows, out long maxTextureId);

            if (textureCount < 1)
                throw new IOException("Cannot process textures. No glyphs specified.");
            if (maxTextureId > 4096)
                throw new IndexOutOfRangeException("The texture ids are too large to be inserted in a Positional Atlas.");

            List<AtlasPointer> pointers = new List<AtlasPointer>((int)maxTextureId);

            if (maxTextureId > 2)
            {
                double x = Math.Sqrt(maxTextureId);
                if (x > (int)x)
                    texturesPerRow = (int)x + 1;
                else
                    texturesPerRow = (int)x;

                x = maxTextureId / (double)texturesPerRow;
                if (x > (int)x)
                    rows = (int)x + 1;
                else
                    rows = (int)x;
            }
            else
            {
                texturesPerRow = 2;
                rows = 1;
            }

            Bitmap buffer = new Bitmap(textureW * texturesPerRow, textureH * rows, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Graphics device = Graphics.FromImage(buffer);
            device.Clear(Color.White);
            device.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            foreach (IIndexableTexture tex in textures)
            {
                int i = (int)tex.Id;

                int y = i / texturesPerRow;
                int x = i % texturesPerRow;

                int xpos = x * textureW;
                int ypos = y * textureH;
                xpos += (int)(textureW / 2.0f);
                ypos += (int)(textureH / 2.0f);
                xpos -= (int)(tex.Size.Width / 2.0f);
                ypos -= (int)(tex.Size.Height / 2.0f);

                pointers.Add(new AtlasPointer(tex.Id, new Point(xpos, ypos), tex.Size));

                device.DrawImage(tex.Bitmap, new Rectangle(xpos, ypos, tex.Size.Width, tex.Size.Height), new Rectangle(Point.Empty, tex.Size), GraphicsUnit.Pixel);
            }

            device.Flush();
            device.Dispose();

            return new Atlas(buffer, AtlasLayoutMode.Positional, pointers.ToArray());
        }
    }
}
