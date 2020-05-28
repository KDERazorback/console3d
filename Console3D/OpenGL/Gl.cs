#if EMBEDDED_GL
using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D.OpenGL
{
    public static partial class Gl
    {
        public static bool IsApiBound { get; private set; }
        public static void BindApi()
        {
            _Internal_BindApi();

            IsApiBound = true;
        }

        public static void UnbindApi()
        {
            IsApiBound = false;

            _Internal_UnbindApi();
        }
    }
}
#endif