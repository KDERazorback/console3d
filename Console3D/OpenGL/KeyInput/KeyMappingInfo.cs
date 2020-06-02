using OpenToolkit.Windowing.Common.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D.OpenGL.KeyInput
{
    public class KeyMappingInfo
    {
        public int KeyCode { get; }
        public KeyStateGroup[] KeyConstrains { get; }
        public string Value { get; }

        public KeyMappingInfo(int code, string value, params KeyStateGroup[] constrains)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException(nameof(value));

            KeyCode = code;
            Value = value;
            KeyConstrains = constrains;
        }

        public bool IsMatch(KeyboardState state)
        {
            foreach (KeyStateGroup group in KeyConstrains)
                if (!group.IsMatch(state))
                    return false;

            return true;
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("0x");
            str.Append(KeyCode.ToString("X"));
            str.Append("-->'");
            str.Append(Value);
            str.Append("' @ ");

            for (int i = 0; i < KeyConstrains.Length; i++)
            {
                if (i > 0)
                    str.Append("+");

                str.Append(KeyConstrains[i].ToString());
            }

            return str.ToString();
        }
    }
}
