using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace Console3D.Textures.TextureAtlas
{
    public class Atlas
    {
        protected SortedList<long, AtlasPointer> SortedPointers { get; set; }

        public Atlas(Bitmap bmp, AtlasLayoutMode mode, IEnumerable<AtlasPointer> pointers)
        {
            if (bmp == null)
                throw new ArgumentNullException("The bitmap parameter for the Atlas cannot be null.");
            if (pointers == null)
                throw new ArgumentNullException("The pointers list for the Atlas cannot be null.");

            SortedPointers = new SortedList<long, AtlasPointer>(4096);
            foreach (AtlasPointer pointer in pointers)
                SortedPointers.Add(pointer.Id, pointer);

            Data = bmp;
            LayoutMode = mode;
        }

        public Bitmap Data { get; }
        public AtlasLayoutMode LayoutMode { get; }
        public AtlasPointer[] Pointers
        {
            get
            {
                return SortedPointers.Values.ToArray();
            }
        }
        public int Count => SortedPointers.Count;
        public Size TextureSize => Pointers[0].Size;
        public Size AtlasTextureSize => Data.Size;

        public AtlasPointer GetPointerById(int id)
        {
            return SortedPointers[id];
        }

        public void ToFile(string bitmapFilename, string metadataFilename, ImageFormat bitmapFormat)
        {
            if (string.IsNullOrWhiteSpace(bitmapFilename))
                throw new ArgumentNullException("The bitmap filename cannot be null.");
            if (string.IsNullOrWhiteSpace(metadataFilename))
                throw new ArgumentNullException("The metadata filename cannot be null.");

            FileInfo bitmapFile = new FileInfo(bitmapFilename.Replace('/', Path.DirectorySeparatorChar));
            FileInfo metadataFile = new FileInfo(metadataFilename.Replace('/', Path.DirectorySeparatorChar));

            if (!bitmapFile.Directory.Exists)
                bitmapFile.Directory.Create();
            if (!metadataFile.Directory.Exists)
                metadataFile.Directory.Create();

            Data.Save(bitmapFile.FullName, bitmapFormat);

            using (FileStream fs = new FileStream(metadataFile.FullName, FileMode.Create, FileAccess.Write, FileShare.None))
            using (StreamWriter writer = new StreamWriter(fs, Encoding.ASCII))
            {
                writer.WriteLine(LayoutMode.ToString());
                foreach (AtlasPointer pointer in Pointers)
                    writer.WriteLine(pointer.ToString());
            }
        }

        public static Atlas FromFile(string bitmapFilename, string metadataFilename)
        {
            if (string.IsNullOrWhiteSpace(bitmapFilename))
                throw new ArgumentNullException("The bitmap filename cannot be null.");
            if (string.IsNullOrWhiteSpace(metadataFilename))
                throw new ArgumentNullException("The metadata filename cannot be null.");

            FileInfo bitmapFile = new FileInfo(bitmapFilename.Replace('/', Path.DirectorySeparatorChar));
            FileInfo metadataFile = new FileInfo(metadataFilename.Replace('/', Path.DirectorySeparatorChar));

            if (!bitmapFile.Exists)
                throw new FileNotFoundException("The specified bitmap file cannot be found.");
            if (!metadataFile.Exists)
                throw new FileNotFoundException("The specified metadata file cannot be found.");

            Bitmap bmp = (Bitmap)Bitmap.FromFile(bitmapFile.FullName);
            AtlasLayoutMode mode;

            List<AtlasPointer> pointers = new List<AtlasPointer>();

            using (FileStream fs = new FileStream(metadataFile.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (StreamReader reader = new StreamReader(fs, Encoding.ASCII))
            {
                mode = Enum.Parse<AtlasLayoutMode>(reader.ReadLine().Trim(), true);

                while (true)
                {
                    string line = reader.ReadLine();

                    if (line == null)
                        break;

                    string[] segments = line.Split(';');

                    long id = long.Parse(segments[0]);
                    string offset = segments[1];
                    string size = segments[2];

                    segments = offset.Split(',');
                    int x = int.Parse(segments[0]);
                    int y = int.Parse(segments[1]);

                    segments = size.Split(',');
                    int w = int.Parse(segments[0]);
                    int h = int.Parse(segments[1]);

                    pointers.Add(new AtlasPointer(id, new Point(x, y), new Size(w, h)));
                }
            }

            return new Atlas(bmp, mode, pointers);
        }
    }
}
