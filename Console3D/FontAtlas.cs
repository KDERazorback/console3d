using Console3D.Textures.Text;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Console3D
{
    public static class FontAtlas
    {
        public static FileInfo GetAtlasFileFromName(string cacheDir, string familyName)
        {
            cacheDir = cacheDir.Replace('/', Path.DirectorySeparatorChar);
            FileInfo targetFile = new FileInfo(Path.Combine(cacheDir, GetFontCachedName(familyName)));

            return targetFile;
        }

        public static FileInfo GetAtlasMetadataFileFromName(string cacheDir, string familyName)
        {
            cacheDir = cacheDir.Replace('/', Path.DirectorySeparatorChar);

            FileInfo targetFileMetadata = new FileInfo(Path.Combine(cacheDir, Path.GetFileNameWithoutExtension(GetFontCachedName(familyName)) + ".dat"));

            return targetFileMetadata;
        }

        public static string GetFontCachedName(FontFamily family)
        {
            return GetFontCachedName(family.Name);
        }

        public static string GetFontCachedName(string familyName)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            string filename = string.Format("{0}.bmp", familyName);
            foreach (char c in invalidChars)
                filename = filename.Replace(c, '_');

            return filename;
        }
    }
}
