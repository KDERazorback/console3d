using OpenToolkit.Windowing.Common.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D.OpenGL
{
    public static class KeyConverter
    {
        public static char? KeyToChar(Key key, bool capsLockState, bool shiftState)
        {
            int code = (int)key;
            bool isUpper = capsLockState ^ shiftState;

            if (code >= 0x54 && code <= 0x6D)
            {
                if (isUpper)
                    return (char)(code - 0x13);

                return (char)(code + 0x0D);
            }

            if (code >= 0x6E && code <= 0x77)
                return (char)(code - 0x3E);

            if (code == 0x34)
                return ' ';

            return null; // Ignore key
        }
    }
}
