using com.RazorSoftware.Logging;
using OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Console3D.Textures.Text
{
    public class GdiFontRasterizer
    {
        protected SortedList<int, GlyphInfo> SortedUnicodeTable;

        public Font SelectedFont { get; set; }
        protected SlimLogger Logger { get; }
        public bool RasterAscii { get; } = true; // From [0x20-0x7E]
        public bool RasterBasicUtf8 { get; } = true; // From [0xA1-0x36F]
        public bool RasterGreekBasedUtf8 { get; } = true; // From [0x370-0x58F]
        public string UnicodeTableFilename { get; protected set; } = "./res/UnicodeData.txt";
        public GlyphInfo[] UnicodeTableData
        {
            get
            {
                return SortedUnicodeTable.Values.ToArray();
            }
            set
            {
                SortedUnicodeTable = new SortedList<int, GlyphInfo>(value.Length);
                foreach (var g in value)
                    SortedUnicodeTable.Add(g.CodePoint, g);
            }
        }

        public GdiFontRasterizer(bool ascii, bool basicUtf8, bool greekUtf8, string logFilename = "./logs/font_rasterizer.log")
        {
            RasterAscii = ascii;
            RasterBasicUtf8 = basicUtf8;
            RasterGreekBasedUtf8 = greekUtf8;

            Logger = new SlimLogger(logFilename.Replace('/', Path.DirectorySeparatorChar), "FONTRASTER");
        }
        public GdiFontRasterizer(GlyphInfo[] unicodeTable, string logFilename = "./logs/font_rasterizer.log") : this(false, false, false, logFilename)
        {
            if (unicodeTable == null || unicodeTable.Length < 1)
                throw new ArgumentNullException("The specified Unicode Table parameter cannot be Null or empty.");

            UnicodeTableData = unicodeTable;
        }

        public GlyphCollection Raster()
        {
            if (SelectedFont == null)
                throw new ArgumentNullException("The specified font to raster is Null.");

            Logger.WriteLine("Starting font rasterization for %@", LogLevel.Message, SelectedFont.ToString());

            if (SortedUnicodeTable == null || SortedUnicodeTable.Count < 1)
                LoadUnicodeTable();

            SortedList<int, Glyph> glyphs = new SortedList<int, Glyph>(UnicodeTableData.Length);

            long errors = 0;
            long warnings = 0;
            Bitmap buffer = new Bitmap(1024, 1024, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Graphics device = Graphics.FromImage(buffer);
            device.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
            SizeF lastGlyphSize = SizeF.Empty;
            foreach (var info in UnicodeTableData)
            {
                try
                {
                    device.Clear(Color.White);
                    SizeF glyphSize = device.MeasureString(info.Literal, SelectedFont, PointF.Empty, StringFormat.GenericTypographic);
                    if (glyphSize.IsEmpty)
                    {
                        Logger.WriteLine("Cannot rasterize Glyph with code %@. Glyph not found inside source Font.", LogLevel.Warning, info.CodePoint);
                        warnings++;
                        continue;
                    }
                    if (glyphSize.Width < 1)
                    {
                        Logger.WriteLine("Glyph %@ doesnt have a valid width after string measurement. Using previous glyph as template with value of %@.", LogLevel.Warning, info.CodePoint, lastGlyphSize.Width);
                        glyphSize.Width = lastGlyphSize.Width;
                        warnings++;
                    }
                    if (glyphSize.Height < 1)
                    {
                        Logger.WriteLine("Glyph %@ doesnt have a valid height after string measurement. Using previous glyph as template  with value of %@.", LogLevel.Warning, info.CodePoint, lastGlyphSize.Height);
                        glyphSize.Height = lastGlyphSize.Height;
                        warnings++;
                    }

                    if (info.CodePoint == 32)
                        continue; // Skip white-space

                    if (glyphSize.Width < 1 || glyphSize.Height < 1)
                    {
                        Logger.WriteLine("Cannot rasterize glyph with code %@. Cannot properly measure glyph size.", LogLevel.Error, info.CodePoint);
                        errors++;
                        continue;
                    }

                    lastGlyphSize = glyphSize;

                    Bitmap bmp = new Bitmap((int)Math.Round(glyphSize.Width, 0), (int)Math.Round(glyphSize.Height, 0), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    Graphics g = Graphics.FromImage(bmp);
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    g.Clear(Color.White);
                    g.DrawString(info.Literal, SelectedFont, Brushes.Black, Point.Empty, StringFormat.GenericTypographic);
                    g.Flush();
                    g.Dispose();

                    glyphs.Add(info.CodePoint, new Glyph(info, bmp));
                    bmp = null;
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("Cannot raster Glyph with code %@. [%@] %@", LogLevel.Error, info.CodePoint, ex.GetType().Name, ex.Message);
                    errors++;
                    continue;
                }
            }

            device.Dispose();
            buffer.Dispose();

            Logger.WriteLine("Rasterized %@ glyphs using Font %@.", LogLevel.Message, glyphs.Count.ToString(), SelectedFont.ToString());
            if (errors > 0)
                Logger.WriteLine("Could not rasterize %@ glyphs due to rasterization errors.", LogLevel.Warning, errors.ToString());
            else
                Logger.WriteLine("0 errors during the rasterization process.", LogLevel.Message);
            Logger.WriteLine("%@ warnings found during the rasterization process.", (warnings > 0 ? LogLevel.Warning : LogLevel.Message), warnings.ToString());

            return new GlyphCollection(glyphs);
        }

        protected void LoadUnicodeTable()
        {
            FileInfo fi = new FileInfo(UnicodeTableFilename.Replace('/', Path.DirectorySeparatorChar));

            Logger.WriteLine("Loading unicode table file from %@", LogLevel.Message, fi.FullName);
            Logger.WriteLine("Filter: Ascii=%@; BasicUTF8=%@; GreekBasedUTF8=%@", LogLevel.Message, RasterAscii.ToString(), RasterBasicUtf8.ToString(), RasterGreekBasedUtf8.ToString());

            if (!fi.Exists)
                throw new FileNotFoundException("Cannot locate the required Unicode table for Font rasterization.");

            SortedUnicodeTable = new SortedList<int, GlyphInfo>(8192);

            long lineIndex = -1;
            using (FileStream fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (StreamReader reader = new StreamReader(fs))
            {
                while (true)
                {
                    lineIndex++;

                    string line = reader.ReadLine();

                    if (line == null)
                        break; // EOF

                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    line = line.Trim();

                    if (line.StartsWith("//"))
                        continue; // Comment

                    string[] segments = line.Split(';');
                    if (segments.Length < 2)
                        continue; // Invalid line

                    int code = 0;
                    string description = null;
                    try
                    {
                        code = int.Parse(segments[0].Trim(), NumberStyles.AllowHexSpecifier);
                        description = segments[1].Trim().ToUpperInvariant();
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine("Failed to process line %@ of Unicode Table. [%@] %@.", LogLevel.Error, lineIndex.ToString(), ex.GetType().Name, ex.Message);
                        continue;
                    }

                    if (description.Contains('<') || description.Contains('>'))
                        continue; // Reserved char

                    if (string.IsNullOrWhiteSpace(description))
                        description = "UNKNOWN";

                    if (RasterAscii &&
                        code >= 0x20 && code <= 0x7E)
                    {
                        SortedUnicodeTable.Add(code, new GlyphInfo(code, description));
                        continue;
                    }

                    if (RasterBasicUtf8 &&
                        code >= 0xA1 && code <= 0x36F)
                    {
                        SortedUnicodeTable.Add(code, new GlyphInfo(code, description));
                        continue;
                    }

                    if (RasterGreekBasedUtf8 &&
                        code >= 0x370 && code <= 0x58F)
                    {
                        SortedUnicodeTable.Add(code, new GlyphInfo(code, description));
                        continue;
                    }

                    // Character outside of bounds
                }
            }

            Logger.WriteLine("%@ entries loaded from the Unicode Table.", LogLevel.Message, SortedUnicodeTable.Count.ToString("N0"));
        }
    }
}
