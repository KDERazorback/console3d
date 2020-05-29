using com.RazorSoftware.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Console3D.Textures.Text
{
    public static class FontLoader
    {
        private static PrivateFontCollection PrivateCollection = new PrivateFontCollection();
        private static SortedSet<string> LoadedFontsSet = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);

        public static FontFamily[] LoadedFonts => PrivateCollection.Families;

        public static void LoadFromFile(string filename)
        {
            filename = filename.Replace('/', Path.DirectorySeparatorChar);
            FileInfo fi = new FileInfo(filename);

            if (!fi.Exists)
                throw new FileNotFoundException("The specified font file {0} cannot be located.", fi.FullName);

            MD5 hasher = MD5.Create();

            byte[] hashData = null;
            using (FileStream fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                hashData = hasher.ComputeHash(fs);

            string hashString = null;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashData.Length; i++)
                builder.Append(hashData[i].ToString("X2"));

            hashString = builder.ToString().Trim();

            if (LoadedFontsSet.Contains(hashString))
                return; // Font already loaded

            PrivateCollection.AddFontFile(fi.FullName);
            LoadedFontsSet.Add(hashString);
        }
    }
}
