using Console3D.Textures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Console3D.Textures.Text
{
    public class GlyphCollection : IEnumerable<Glyph>, IEnumerable<IIndexableTexture>
    {
        public SortedList<int, Glyph> Glyphs { get; }
        public int Count => Glyphs.Count;

        public GlyphCollection(SortedList<int, Glyph> glyphs)
        {
            Glyphs = glyphs;
        }

        public IEnumerator<Glyph> GetEnumerator()
        {
            return Glyphs.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<IIndexableTexture> IEnumerable<IIndexableTexture>.GetEnumerator()
        {
            foreach (Glyph glyph in Glyphs.Values)
                yield return glyph;
        }
    }
}
