
// MIT License
// 
// Copyright (c) 2009-2017 Luca Piccioni
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// 
// This file is automatically generated

#pragma warning disable 649, 1572, 1573

// ReSharper disable RedundantUsingDirective
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

using Khronos;

// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable JoinDeclarationAndInitializer

namespace OpenGL
{
	public partial class Gl
	{
		/// <summary>
		/// [GL] Value of GL_COLOR_ATTACHMENT_EXT symbol.
		/// </summary>
		[RequiredByFeature("GL_EXT_multiview_draw_buffers", Api = "gles2")]
		public const int COLOR_ATTACHMENT_EXT = 0x90F0;

		/// <summary>
		/// [GL] Value of GL_MULTIVIEW_EXT symbol.
		/// </summary>
		[RequiredByFeature("GL_EXT_multiview_draw_buffers", Api = "gles2")]
		public const int MULTIVIEW_EXT = 0x90F1;

		/// <summary>
		/// [GL] Value of GL_MAX_MULTIVIEW_BUFFERS_EXT symbol.
		/// </summary>
		[RequiredByFeature("GL_EXT_multiview_draw_buffers", Api = "gles2")]
		public const int MAX_MULTIVIEW_BUFFERS_EXT = 0x90F2;

		/// <summary>
		/// [GL] glReadBufferIndexedEXT: Binding for glReadBufferIndexedEXT.
		/// </summary>
		/// <param name="src">
		/// A <see cref="T:ReadBufferMode"/>.
		/// </param>
		/// <param name="index">
		/// A <see cref="T:int"/>.
		/// </param>
		[RequiredByFeature("GL_EXT_multiview_draw_buffers", Api = "gles2")]
		public static void ReadBufferIndexedEXT(ReadBufferMode src, int index)
		{
			Debug.Assert(Delegates.pglReadBufferIndexedEXT != null, "pglReadBufferIndexedEXT not implemented");
			Delegates.pglReadBufferIndexedEXT((int)src, index);
			LogCommand("glReadBufferIndexedEXT", null, src, index			);
			DebugCheckErrors(null);
		}

		/// <summary>
		/// [GL] glDrawBuffersIndexedEXT: Binding for glDrawBuffersIndexedEXT.
		/// </summary>
		/// <param name="location">
		/// A <see cref="T:int[]"/>.
		/// </param>
		/// <param name="indices">
		/// A <see cref="T:int[]"/>.
		/// </param>
		[RequiredByFeature("GL_EXT_multiview_draw_buffers", Api = "gles2")]
		public static void DrawBuffersIndexedEXT(int[] location, int[] indices)
		{
			unsafe {
				fixed (int* p_location = location)
				fixed (int* p_indices = indices)
				{
					Debug.Assert(Delegates.pglDrawBuffersIndexedEXT != null, "pglDrawBuffersIndexedEXT not implemented");
					Delegates.pglDrawBuffersIndexedEXT(location.Length, p_location, p_indices);
					LogCommand("glDrawBuffersIndexedEXT", null, location.Length, location, indices					);
				}
			}
			DebugCheckErrors(null);
		}

		/// <summary>
		/// [GL] glGetIntegeri_vEXT: Binding for glGetIntegeri_vEXT.
		/// </summary>
		/// <param name="target">
		/// A <see cref="T:GetPName"/>.
		/// </param>
		/// <param name="index">
		/// A <see cref="T:uint"/>.
		/// </param>
		/// <param name="data">
		/// A <see cref="T:int[]"/>.
		/// </param>
		[RequiredByFeature("GL_EXT_multiview_draw_buffers", Api = "gles2")]
		public static void GetIntegerEXT(GetPName target, uint index, [Out] int[] data)
		{
			unsafe {
				fixed (int* p_data = data)
				{
					Debug.Assert(Delegates.pglGetIntegeri_vEXT != null, "pglGetIntegeri_vEXT not implemented");
					Delegates.pglGetIntegeri_vEXT((int)target, index, p_data);
					LogCommand("glGetIntegeri_vEXT", null, target, index, data					);
				}
			}
			DebugCheckErrors(null);
		}

		internal static unsafe partial class Delegates
		{
			[RequiredByFeature("GL_EXT_multiview_draw_buffers", Api = "gles2")]
			[SuppressUnmanagedCodeSecurity]
			internal delegate void glReadBufferIndexedEXT(int src, int index);

			[RequiredByFeature("GL_EXT_multiview_draw_buffers", Api = "gles2")]
			[ThreadStatic]
			internal static glReadBufferIndexedEXT pglReadBufferIndexedEXT;

			[RequiredByFeature("GL_EXT_multiview_draw_buffers", Api = "gles2")]
			[SuppressUnmanagedCodeSecurity]
			internal delegate void glDrawBuffersIndexedEXT(int n, int* location, int* indices);

			[RequiredByFeature("GL_EXT_multiview_draw_buffers", Api = "gles2")]
			[ThreadStatic]
			internal static glDrawBuffersIndexedEXT pglDrawBuffersIndexedEXT;

			[RequiredByFeature("GL_EXT_multiview_draw_buffers", Api = "gles2")]
			[SuppressUnmanagedCodeSecurity]
			internal delegate void glGetIntegeri_vEXT(int target, uint index, int* data);

			[RequiredByFeature("GL_EXT_multiview_draw_buffers", Api = "gles2")]
			[ThreadStatic]
			internal static glGetIntegeri_vEXT pglGetIntegeri_vEXT;

		}
	}

}