using OpenToolkit.Windowing.Common;
using OpenToolkit.Windowing.Common.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D
{
    // Reimplementation of KeyboardKeyEventArgs
    public class ConsoleRenderProgramKeyEventArgs
    {
        internal ConsoleRenderProgramKeyEventArgs(KeyboardKeyEventArgs obj)
        {
            Shift = obj.Shift;
            Control = obj.Control;
            Alt = obj.Alt;
            Command = obj.Command;
            IsRepeat = obj.IsRepeat;
            Key = obj.Key;
            KeyModifiers = obj.Modifiers;
            ScanCode = obj.ScanCode;
        }
        public bool Shift { get; set; }
        public KeyModifiers KeyModifiers { get; set; }

        public bool Intercept { get; set; } = false;
        public Key Key { get; set; }
        public bool IsRepeat { get; set; }
        public bool Alt { get; set; }
        public bool Command { get; set; }
        public bool Control { get; set; }
        public int ScanCode { get; set; }
    }
}
