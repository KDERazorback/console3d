using OpenToolkit.Windowing.Common.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D.OpenGL.KeyInput
{
    public class KeyStateGroup
    {
        public KeyValuePair<int, bool>[] KeyStates { get; }
        public KeyStateGroup[] SubGroups { get; }
        public bool MatchAny { get; }

        public KeyStateGroup(bool matchAny, KeyStateGroup[] subgroups, params KeyValuePair<int, bool>[] keys)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));

            KeyStates = keys;
            SubGroups = subgroups;
            MatchAny = matchAny;
        }

        public bool IsMatch(KeyboardState state, bool recursive = true)
        {
            bool anyMatch = false;
            bool allMatch = true;

            foreach (var entry in KeyStates)
            {
                if (state.IsKeyDown((Key)entry.Key) == entry.Value)
                    anyMatch = true;
                else
                    allMatch = false;

                if (!MatchAny && !allMatch)
                    return false;
            }

            if (recursive)
            {
                foreach (KeyStateGroup subgroup in SubGroups)
                {
                    if (subgroup.IsMatch(state, recursive))
                        anyMatch = true;
                    else
                        allMatch = false;

                    if (!MatchAny && !allMatch)
                        return false;
                }
            }

            if (MatchAny && anyMatch)
                return true;

            if (!MatchAny && allMatch)
                return true;

            return false;
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();

            str.Append("{");
            bool first = true;
            foreach (var entry in KeyStates)
            {
                if (!first)
                    str.Append(MatchAny ? "|" : "+");

                if (!entry.Value)
                    str.Append("!");

                if (Enum.TryParse<Key>(entry.Key.ToString(), out Key name))
                    str.Append(name);
                else
                {
                    str.Append("[0x");
                    str.Append(entry.Key.ToString("X"));
                    str.Append("]");
                }

                first = false;
            }
            str.Append("}");

            foreach (var subentry in SubGroups)
            {
                str.Append("+");
                str.Append(subentry.ToString());
            }

            return str.ToString();
        }
    }
}
