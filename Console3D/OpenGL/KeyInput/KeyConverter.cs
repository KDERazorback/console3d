using OpenToolkit.Windowing.Common.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace Console3D.OpenGL.KeyInput
{
    public class KeyConverter
    {
        public static KeyConverter Default = KeyConverter.FailSafe;

        public static KeyConverter FailSafe
        {
            get
            {
                return new KeyConverter("failsafe") { IsFailsafe = true, IsLoaded = true };
            }
        }

        private Dictionary<int, KeyMappingInfo> CustomMappings = new Dictionary<int, KeyMappingInfo>(128);
        public KeyConverter(string layout)
        {
            Layout = layout;
        }

        public bool IgnoreLoadErrors { get; set; } = false;
        public bool IsLoaded { get; private set; }
        public string Layout { get; }
        public bool IsFailsafe { get; private set; }
        private SortedList<long, InternalKeyMappingEntry> InternalMappings { get; set; }
        public void AddCustomMapping(int code, string value, params KeyStateGroup[] constraints)
        {
            if (!IsLoaded)
                throw new InvalidOperationException("A keyboard layout must be loaded before calling any of the KeyConverter methods.");

            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));

            if (IsMapped(code))
                throw new InvalidOperationException("The value " + code.ToString() + " is already mapped either internally or by a custom mapping defined earlier.");

            CustomMappings.Add(code, new KeyMappingInfo(code, value, constraints));
        }

        public void ClearCustomMappings()
        {
            if (!IsLoaded)
                throw new InvalidOperationException("A keyboard layout must be loaded before calling any of the KeyConverter methods.");

            CustomMappings.Clear();
        }

        public KeyMappingInfo GetCustomKeyMapping(Key key)
        {
            return GetCustomKeyMapping((int)key);
        }

        public KeyMappingInfo GetCustomKeyMapping(int key)
        {
            if (!IsLoaded)
                throw new InvalidOperationException("A keyboard layout must be loaded before calling any of the KeyConverter methods.");

            return CustomMappings[key];
        }

        public KeyValuePair<int, KeyMappingInfo>[] GetCustomMappings()
        {
            if (!IsLoaded)
                throw new InvalidOperationException("A keyboard layout must be loaded before calling any of the KeyConverter methods.");

            List<KeyValuePair<int, KeyMappingInfo>> entries = new List<KeyValuePair<int, KeyMappingInfo>>(CustomMappings.Count);

            foreach (var entry in CustomMappings)
                entries.Add(new KeyValuePair<int, KeyMappingInfo>(entry.Key, entry.Value));

            return entries.ToArray();
        }

        public bool IsCustomMapped(int code)
        {
            if (!IsLoaded)
                throw new InvalidOperationException("A keyboard layout must be loaded before calling any of the KeyConverter methods.");

            return CustomMappings.ContainsKey(code);
        }

        public bool IsMapped(int keyCode)
        {
            if (!IsLoaded)
                throw new InvalidOperationException("A keyboard layout must be loaded before calling any of the KeyConverter methods.");

            string internalMap = InternalKeyToString(keyCode, 0, false, false);
            if (internalMap != null)
                return true;

            return IsCustomMapped(keyCode);
        }

        public bool IsMapped(Key key)
        {
            return IsMapped((int)key);
        }

        public string KeyToString(Key key)
        {
            return KeyToString(key, false, false, false);
        }

        public string KeyToString(Key key, bool capsLockState, bool shiftState, bool controlState)
        {
            KeyboardState state = new KeyboardState();
            state.SetKeyState(key, true);
            state.SetKeyState(Key.LShift, shiftState);
            state.SetKeyState(Key.LControl, controlState);

            return KeyToString((int)key, state, capsLockState);
        }

        public string KeyToString(Key key, bool capsLockState, bool shiftState)
        {
            return KeyToString(key, capsLockState, shiftState, false);
        }

        public string KeyToString(int keyCode, KeyboardState keyboardState, bool capsLockState)
        {
            if (!IsLoaded)
                throw new InvalidOperationException("A keyboard layout must be loaded before calling any of the KeyConverter methods.");

            string internalMap = InternalKeyToString(keyCode, keyboardState, capsLockState);

            if (internalMap != null)
                return internalMap;

            if (IsCustomMapped(keyCode))
            {
                KeyMappingInfo info = GetCustomKeyMapping(keyCode);

                if (info.IsMatch(keyboardState))
                    return info.Value;
            }

            return null;
        }

        public void LoadLayout(Stream sourceStream)
        {
            if (IsLoaded)
                throw new InvalidOperationException("This KeyConverter already have a loaded layout file.");

            using (StreamReader reader = new StreamReader(sourceStream))
            {
                int lineIndex = 0;
                SortedList<long, InternalKeyMappingEntry> entries = new SortedList<long, InternalKeyMappingEntry>(512);
                while (true)
                {
                    string line = reader.ReadLine();

                    if (line == null)
                        break;

                    lineIndex++;

                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    line = line.Trim();

                    if (line.StartsWith("#"))
                        continue; // Comment

                    string[] segments = line.Split(';', 3);

                    if (segments.Length != 3)
                    {
                        if (IgnoreLoadErrors)
                            continue;

                        throw new IOException(string.Format("Cannot process entry at line {0}. Unexpected segment count {1}.", lineIndex.ToString(), segments?.Length.ToString() ?? "NULL"));
                    }

                    try
                    {
                        int code;
                        string charcode = segments[0].Trim();
                        if (charcode.StartsWith("0x"))
                            code = Convert.ToInt32(charcode.Substring(2), 16);
                        else
                            code = int.Parse(charcode);

                        bool altgr = false;
                        bool? isUpper = null;
                        KeyModifiers modifiers = 0;
                        if (segments[1].Contains('s', StringComparison.OrdinalIgnoreCase)) modifiers |= KeyModifiers.Shift;
                        if (segments[1].Contains('c', StringComparison.OrdinalIgnoreCase)) modifiers |= KeyModifiers.Control;
                        if (segments[1].Contains('m', StringComparison.OrdinalIgnoreCase)) modifiers |= KeyModifiers.Command;
                        if (segments[1].Contains('a', StringComparison.OrdinalIgnoreCase)) modifiers |= KeyModifiers.Alt;
                        if (segments[1].Contains('g', StringComparison.OrdinalIgnoreCase)) altgr = true;
                        if (segments[1].Contains('u', StringComparison.OrdinalIgnoreCase)) isUpper = true;
                        if (segments[1].Contains('l', StringComparison.OrdinalIgnoreCase)) isUpper = false;

                        long id = GetInternalKeyId(code, modifiers, altgr, isUpper);

                        string value;
                        if (segments[2].Length == 1)
                            value = segments[2];
                        else
                            value = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(segments[2].Trim()));

                        entries.Add(id, new InternalKeyMappingEntry(code, modifiers, value));
                    }
                    catch (Exception)
                    {
                        if (IgnoreLoadErrors)
                            continue;

                        throw;
                    }
                }

                InternalMappings = entries;
            }

            IsLoaded = true;
        }

        public void RemoveCustomMapping(int code)
        {
            if (!IsLoaded)
                throw new InvalidOperationException("A keyboard layout must be loaded before calling any of the KeyConverter methods.");

            CustomMappings.Remove(code);
        }
        private string InternalKeyToString(int code, KeyboardState keyboardState, bool capsLockState)
        {
            bool shiftState = keyboardState.IsKeyDown(Key.LShift) || keyboardState.IsKeyDown(Key.RShift);
            bool controlState = keyboardState.IsKeyDown(Key.LControl) || keyboardState.IsKeyDown(Key.RControl);
            bool altState = keyboardState.IsKeyDown(Key.LAlt);
            bool altgrState = keyboardState.IsKeyDown(Key.RAlt);
            bool commandState = keyboardState.IsKeyDown(Key.Command);

            bool isUpper = capsLockState ^ shiftState;

            KeyModifiers modifiers = 0;
            if (shiftState) modifiers |= KeyModifiers.Shift;
            if (controlState) modifiers |= KeyModifiers.Control;
            if (altState) modifiers |= KeyModifiers.Alt;
            if (commandState) modifiers |= KeyModifiers.Command;

            return InternalKeyToString(code, modifiers, altgrState, isUpper);
        }

        private string InternalKeyToString(int code, KeyModifiers modifiers, bool altgr, bool? isUpper)
        {
            if (IsFailsafe)
                return InternalKeyToString_Failsafe(code, modifiers, altgr, isUpper);

            return InternalKeyToString_Keymap(code, modifiers, altgr, isUpper);
        }

        private long GetInternalKeyId(int code, KeyModifiers modifiers, bool altgr, bool? upper)
        {
            int customFlags = 0;
            if (altgr) customFlags += 1; // AltGr
            if (upper.HasValue)
            {
                if (upper.Value)
                    customFlags += 2; // IsUpper
                else
                    customFlags += 4; // IsLower
            }


            return code + ((int)modifiers << 16) + (customFlags << 24);
        }

        private string InternalKeyToString_Keymap(int code, KeyModifiers modifiers, bool altgr, bool? isUpper)
        {
            long id = GetInternalKeyId(code, modifiers, altgr, isUpper);

            if (InternalMappings.TryGetValue(id, out InternalKeyMappingEntry entry))
                return entry.Value;

            if (isUpper.HasValue && !isUpper.Value)
            {
                id = GetInternalKeyId(code, modifiers, altgr, null); // Try to get case-invariant version

                if (InternalMappings.TryGetValue(id, out InternalKeyMappingEntry lcentry))
                    return lcentry.Value;
            }

            return null;
        }

        private string InternalKeyToString_Failsafe(int code, KeyModifiers modifiers, bool altgr, bool? isUpper)
        {
            if (!IsLoaded)
                throw new InvalidOperationException("A keyboard layout must be loaded before calling any of the KeyConverter methods.");

            // Standard Alphabet
            if (code >= 0x54 && code <= 0x6D)
            {
                if (isUpper.HasValue && isUpper.Value)
                    return new string((char)(code - 0x13), 1);

                return new string((char)(code + 0x0D), 1);
            }

            // Numbers
            if (code >= 0x6E && code <= 0x77)
            {
                string numberMappings = ")!@#$%^&*(";

                if (modifiers.HasFlag(KeyModifiers.Shift))
                {
                    int index = code - 0x6E;
                    return new string(numberMappings[index], 1);
                }
                else
                    return new string((char)(code - 0x3E), 1);
            }

            // Numeric KeyPad
            if (code >= 0x44 && code <= 0x4D)
            {
                return new string((char)(code - 0x14), 1);
            }

            // Symbols and key combinations
            if (code == 0x34) return " ";
            if (code == 0x4E) return "/";
            if (code == 0x4F) return "*";
            if (code == 0x50) return "-";
            if (code == 0x51) return "+";
            if (code == 0x52) return ".";
            if (code == 0x78 && modifiers.HasFlag(KeyModifiers.Shift)) return "`";
            if (code == 0x78 && !modifiers.HasFlag(KeyModifiers.Shift)) return "~";
            if (code == 0x79 && modifiers.HasFlag(KeyModifiers.Shift)) return "_";
            if (code == 0x79 && !modifiers.HasFlag(KeyModifiers.Shift)) return "-";
            if (code == 0x7A && modifiers.HasFlag(KeyModifiers.Shift)) return "+";
            if (code == 0x7A && !modifiers.HasFlag(KeyModifiers.Shift)) return "=";
            if (code == 0x7B && modifiers.HasFlag(KeyModifiers.Shift)) return "{";
            if (code == 0x7B && !modifiers.HasFlag(KeyModifiers.Shift)) return "[";
            if (code == 0x7C && modifiers.HasFlag(KeyModifiers.Shift)) return "}";
            if (code == 0x7C && !modifiers.HasFlag(KeyModifiers.Shift)) return "]";
            if (code == 0x7D && modifiers.HasFlag(KeyModifiers.Shift)) return ":";
            if (code == 0x7D && !modifiers.HasFlag(KeyModifiers.Shift)) return ";";
            if (code == 0x7E && modifiers.HasFlag(KeyModifiers.Shift)) return "\"";
            if (code == 0x7E && !modifiers.HasFlag(KeyModifiers.Shift)) return "'";
            if (code == 0x7F && modifiers.HasFlag(KeyModifiers.Shift)) return "<";
            if (code == 0x7F && !modifiers.HasFlag(KeyModifiers.Shift)) return ",";
            if (code == 0x80 && modifiers.HasFlag(KeyModifiers.Shift)) return ">";
            if (code == 0x80 && !modifiers.HasFlag(KeyModifiers.Shift)) return ".";
            if (code == 0x81 && modifiers.HasFlag(KeyModifiers.Shift)) return "?";
            if (code == 0x81 && !modifiers.HasFlag(KeyModifiers.Shift)) return "/";
            if (code == 0x82 && modifiers.HasFlag(KeyModifiers.Shift)) return "|";
            if (code == 0x82 && !modifiers.HasFlag(KeyModifiers.Shift)) return "\\";


            return null; // Ignore key
        }
    }
}