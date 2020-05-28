#if EMBEDDED_GL
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Console3D.OpenGL
{
    public static partial class Gl
    {
        // Binding targets
        private static readonly KeyValuePair<string, string>[] _Internal_DelegateTargets = new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>(nameof(ClearColor), "glClearColor"),
            new KeyValuePair<string, string>(nameof(Clear), "glClear"),
        };

        // Binding
        private static void _Internal_BindApi()
        {
            /* API_GEN_BINDER */
            Type thisType = typeof(Gl);
            foreach (KeyValuePair<string, string> entry in _Internal_DelegateTargets)
            {
                try
                {
                    FieldInfo field = thisType.GetField(entry.Key);
                    IntPtr address = GLFW.Glfw.GetProcAddress(entry.Value);

                    if (address == IntPtr.Zero)
                        throw new InvalidCastException("Cannot locate entrypoint for unmanaged function " + entry.Value);

                    field.SetValue(null, Marshal.GetDelegateForFunctionPointer(address, field.FieldType));
                }
                catch (Exception ex)
                {
                    
                }
            }

            //ClearColor = Marshal.GetDelegateForFunctionPointer<glClearColorHandler>(GLFW.Glfw.GetProcAddress("glClearColor"));
            //Clear = Marshal.GetDelegateForFunctionPointer<glClearHandler>(GLFW.Glfw.GetProcAddress("glClear"));
        }

        // Unbinding
        private static void _Internal_UnbindApi()
        {
            /* API_GEN_UNBINDER */
            ClearColor = null;
            Clear = null;
        }
    }
}
#endif