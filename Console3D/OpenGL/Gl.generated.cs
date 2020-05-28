#if EMBEDDED_GL
using System;
using System.Collections.Generic;
using System.Text;

namespace Console3D.OpenGL
{
    public static partial class Gl
    {
        // Members
        /* API_GEN_MEMBERS */


        // Delegates
        /* API_GEN_DELEGATES */
        public delegate void glClearColorHandler(float r, float g, float b, float a);
        public delegate void glClearHandler(int mask);


        // Methods
        /* API_GEN_METHODS */
        public static glClearColorHandler ClearColor;
        public static glClearHandler Clear;
    }
}
#endif