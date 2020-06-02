using OpenToolkit.Windowing.Common.Input;
using System;

namespace Console3D.OpenGL.KeyInput
{
    internal class InternalKeyMappingEntry
    {
        public InternalKeyMappingEntry(int keyCode, KeyModifiers modifiers, string value)
        {
            KeyCode = keyCode;
            Modifiers = modifiers;
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public int KeyCode { get; }
        public KeyModifiers Modifiers { get; }
        public string Value { get; }
    }
}
