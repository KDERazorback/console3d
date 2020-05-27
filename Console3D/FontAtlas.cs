using Console3D.Textures.Text;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Console3D
{
    internal static class FontAtlas
    {
        public static FileInfo GetAtlasFileFromName(string cacheDir, FontFamily family)
        {
            cacheDir = cacheDir.Replace('/', Path.DirectorySeparatorChar);
            FileInfo targetFile = new FileInfo(Path.Combine(cacheDir, FontLoader.GetFontFamilyCachedName(family)));

            return targetFile;
        }

        public static FileInfo GetAtlasMetadataFileFromName(string cacheDir, FontFamily family)
        {
            cacheDir = cacheDir.Replace('/', Path.DirectorySeparatorChar);

            FileInfo targetFileMetadata = new FileInfo(Path.Combine(cacheDir, Path.GetFileNameWithoutExtension(FontLoader.GetFontFamilyCachedName(family)) + ".dat"));

            return targetFileMetadata;
        }
    }
}
