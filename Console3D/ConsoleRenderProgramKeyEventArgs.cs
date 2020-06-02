using Console3D.OpenGL.KeyInput;
using OpenToolkit.Windowing.Common;
using OpenToolkit.Windowing.Common.Input;

namespace Console3D
{
    // Reimplementation of KeyboardKeyEventArgs
    public class ConsoleRenderProgramKeyEventArgs
    {
        internal ConsoleRenderProgramKeyEventArgs(KeyboardKeyEventArgs obj)
        {
            Key = obj.Key;

            IsShift = obj.Shift;
            IsControl = obj.Control;
            IsAlt = obj.Alt;
            IsCommand = obj.Command;

            KeyModifiers = obj.Modifiers;

            IsRepeat = obj.IsRepeat;
            
            ScanCode = obj.ScanCode;
        }
        public bool IsShift { get; }
        public KeyModifiers KeyModifiers { get; }

        public bool Intercept { get; set; } = false;
        public Key Key { get; }
        public bool IsRepeat { get; }
        public bool IsAlt { get; }
        public bool IsCommand { get; }
        public bool IsControl { get; }
        public int ScanCode { get; }

        public string GetAssociatedString(bool capsLockState, KeyConverter converter)
        {
            KeyboardState state = new KeyboardState();
            state.SetKeyState(Key, true);
            state.SetKeyState(Key.LControl, IsControl);
            state.SetKeyState(Key.LAlt, IsAlt);
            state.SetKeyState(Key.LShift, IsShift);
            state.SetKeyState(Key.Command, IsCommand);

            return converter.KeyToString((int)Key, state, capsLockState);
        }

        public string GetAssociatedString(bool capsLockState)
        {
            return GetAssociatedString(capsLockState, KeyConverter.Default);
        }
    }
}
