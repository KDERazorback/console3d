
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
		/// [GL] glGetTextureHandleNV: Binding for glGetTextureHandleNV.
		/// </summary>
		/// <param name="texture">
		/// A <see cref="T:uint"/>.
		/// </param>
		[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
		public static ulong GetTextureHandleNV(uint texture)
		{
			ulong retValue;

			Debug.Assert(Delegates.pglGetTextureHandleNV != null, "pglGetTextureHandleNV not implemented");
			retValue = Delegates.pglGetTextureHandleNV(texture);
			LogCommand("glGetTextureHandleNV", retValue, texture			);
			DebugCheckErrors(retValue);

			return (retValue);
		}

		/// <summary>
		/// [GL] glGetTextureSamplerHandleNV: Binding for glGetTextureSamplerHandleNV.
		/// </summary>
		/// <param name="texture">
		/// A <see cref="T:uint"/>.
		/// </param>
		/// <param name="sampler">
		/// A <see cref="T:uint"/>.
		/// </param>
		[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
		public static ulong GetTextureSamplerHandleNV(uint texture, uint sampler)
		{
			ulong retValue;

			Debug.Assert(Delegates.pglGetTextureSamplerHandleNV != null, "pglGetTextureSamplerHandleNV not implemented");
			retValue = Delegates.pglGetTextureSamplerHandleNV(texture, sampler);
			LogCommand("glGetTextureSamplerHandleNV", retValue, texture, sampler			);
			DebugCheckErrors(retValue);

			return (retValue);
		}

		/// <summary>
		/// [GL] glMakeTextureHandleResidentNV: Binding for glMakeTextureHandleResidentNV.
		/// </summary>
		/// <param name="handle">
		/// A <see cref="T:ulong"/>.
		/// </param>
		[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
		public static void MakeTextureHandleResidentNV(ulong handle)
		{
			Debug.Assert(Delegates.pglMakeTextureHandleResidentNV != null, "pglMakeTextureHandleResidentNV not implemented");
			Delegates.pglMakeTextureHandleResidentNV(handle);
			LogCommand("glMakeTextureHandleResidentNV", null, handle			);
			DebugCheckErrors(null);
		}

		/// <summary>
		/// [GL] glMakeTextureHandleNonResidentNV: Binding for glMakeTextureHandleNonResidentNV.
		/// </summary>
		/// <param name="handle">
		/// A <see cref="T:ulong"/>.
		/// </param>
		[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
		public static void MakeTextureHandleNonResidentNV(ulong handle)
		{
			Debug.Assert(Delegates.pglMakeTextureHandleNonResidentNV != null, "pglMakeTextureHandleNonResidentNV not implemented");
			Delegates.pglMakeTextureHandleNonResidentNV(handle);
			LogCommand("glMakeTextureHandleNonResidentNV", null, handle			);
			DebugCheckErrors(null);
		}

		/// <summary>
		/// [GL] glGetImageHandleNV: Binding for glGetImageHandleNV.
		/// </summary>
		/// <param name="texture">
		/// A <see cref="T:uint"/>.
		/// </param>
		/// <param name="level">
		/// A <see cref="T:int"/>.
		/// </param>
		/// <param name="layered">
		/// A <see cref="T:bool"/>.
		/// </param>
		/// <param name="layer">
		/// A <see cref="T:int"/>.
		/// </param>
		/// <param name="format">
		/// A <see cref="T:PixelFormat"/>.
		/// </param>
		[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
		public static ulong GetImageHandleNV(uint texture, int level, bool layered, int layer, PixelFormat format)
		{
			ulong retValue;

			Debug.Assert(Delegates.pglGetImageHandleNV != null, "pglGetImageHandleNV not implemented");
			retValue = Delegates.pglGetImageHandleNV(texture, level, layered, layer, (int)format);
			LogCommand("glGetImageHandleNV", retValue, texture, level, layered, layer, format			);
			DebugCheckErrors(retValue);

			return (retValue);
		}

		/// <summary>
		/// [GL] glMakeImageHandleResidentNV: Binding for glMakeImageHandleResidentNV.
		/// </summary>
		/// <param name="handle">
		/// A <see cref="T:ulong"/>.
		/// </param>
		/// <param name="access">
		/// A <see cref="T:int"/>.
		/// </param>
		[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
		public static void MakeImageHandleResidentNV(ulong handle, int access)
		{
			Debug.Assert(Delegates.pglMakeImageHandleResidentNV != null, "pglMakeImageHandleResidentNV not implemented");
			Delegates.pglMakeImageHandleResidentNV(handle, access);
			LogCommand("glMakeImageHandleResidentNV", null, handle, access			);
			DebugCheckErrors(null);
		}

		/// <summary>
		/// [GL] glMakeImageHandleNonResidentNV: Binding for glMakeImageHandleNonResidentNV.
		/// </summary>
		/// <param name="handle">
		/// A <see cref="T:ulong"/>.
		/// </param>
		[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
		public static void MakeImageHandleNonResidentNV(ulong handle)
		{
			Debug.Assert(Delegates.pglMakeImageHandleNonResidentNV != null, "pglMakeImageHandleNonResidentNV not implemented");
			Delegates.pglMakeImageHandleNonResidentNV(handle);
			LogCommand("glMakeImageHandleNonResidentNV", null, handle			);
			DebugCheckErrors(null);
		}

		/// <summary>
		/// [GL] glUniformHandleui64NV: Binding for glUniformHandleui64NV.
		/// </summary>
		/// <param name="location">
		/// A <see cref="T:int"/>.
		/// </param>
		/// <param name="value">
		/// A <see cref="T:ulong"/>.
		/// </param>
		[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
		public static void UniformHandleNV(int location, ulong value)
		{
			Debug.Assert(Delegates.pglUniformHandleui64NV != null, "pglUniformHandleui64NV not implemented");
			Delegates.pglUniformHandleui64NV(location, value);
			LogCommand("glUniformHandleui64NV", null, location, value			);
			DebugCheckErrors(null);
		}

		/// <summary>
		/// [GL] glUniformHandleui64vNV: Binding for glUniformHandleui64vNV.
		/// </summary>
		/// <param name="location">
		/// A <see cref="T:int"/>.
		/// </param>
		/// <param name="value">
		/// A <see cref="T:ulong[]"/>.
		/// </param>
		[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
		public static void UniformHandleNV(int location, ulong[] value)
		{
			unsafe {
				fixed (ulong* p_value = value)
				{
					Debug.Assert(Delegates.pglUniformHandleui64vNV != null, "pglUniformHandleui64vNV not implemented");
					Delegates.pglUniformHandleui64vNV(location, value.Length, p_value);
					LogCommand("glUniformHandleui64vNV", null, location, value.Length, value					);
				}
			}
			DebugCheckErrors(null);
		}

		/// <summary>
		/// [GL] glProgramUniformHandleui64NV: Binding for glProgramUniformHandleui64NV.
		/// </summary>
		/// <param name="program">
		/// A <see cref="T:uint"/>.
		/// </param>
		/// <param name="location">
		/// A <see cref="T:int"/>.
		/// </param>
		/// <param name="value">
		/// A <see cref="T:ulong"/>.
		/// </param>
		[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
		public static void ProgramUniformHandleNV(uint program, int location, ulong value)
		{
			Debug.Assert(Delegates.pglProgramUniformHandleui64NV != null, "pglProgramUniformHandleui64NV not implemented");
			Delegates.pglProgramUniformHandleui64NV(program, location, value);
			LogCommand("glProgramUniformHandleui64NV", null, program, location, value			);
			DebugCheckErrors(null);
		}

		/// <summary>
		/// [GL] glProgramUniformHandleui64vNV: Binding for glProgramUniformHandleui64vNV.
		/// </summary>
		/// <param name="program">
		/// A <see cref="T:uint"/>.
		/// </param>
		/// <param name="location">
		/// A <see cref="T:int"/>.
		/// </param>
		/// <param name="values">
		/// A <see cref="T:ulong[]"/>.
		/// </param>
		[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
		public static void ProgramUniformHandleNV(uint program, int location, ulong[] values)
		{
			unsafe {
				fixed (ulong* p_values = values)
				{
					Debug.Assert(Delegates.pglProgramUniformHandleui64vNV != null, "pglProgramUniformHandleui64vNV not implemented");
					Delegates.pglProgramUniformHandleui64vNV(program, location, values.Length, p_values);
					LogCommand("glProgramUniformHandleui64vNV", null, program, location, values.Length, values					);
				}
			}
			DebugCheckErrors(null);
		}

		/// <summary>
		/// [GL] glIsTextureHandleResidentNV: Binding for glIsTextureHandleResidentNV.
		/// </summary>
		/// <param name="handle">
		/// A <see cref="T:ulong"/>.
		/// </param>
		[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
		public static bool IsTextureHandleResidentNV(ulong handle)
		{
			bool retValue;

			Debug.Assert(Delegates.pglIsTextureHandleResidentNV != null, "pglIsTextureHandleResidentNV not implemented");
			retValue = Delegates.pglIsTextureHandleResidentNV(handle);
			LogCommand("glIsTextureHandleResidentNV", retValue, handle			);
			DebugCheckErrors(retValue);

			return (retValue);
		}

		/// <summary>
		/// [GL] glIsImageHandleResidentNV: Binding for glIsImageHandleResidentNV.
		/// </summary>
		/// <param name="handle">
		/// A <see cref="T:ulong"/>.
		/// </param>
		[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
		public static bool IsImageHandleResidentNV(ulong handle)
		{
			bool retValue;

			Debug.Assert(Delegates.pglIsImageHandleResidentNV != null, "pglIsImageHandleResidentNV not implemented");
			retValue = Delegates.pglIsImageHandleResidentNV(handle);
			LogCommand("glIsImageHandleResidentNV", retValue, handle			);
			DebugCheckErrors(retValue);

			return (retValue);
		}

		internal static unsafe partial class Delegates
		{
			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[SuppressUnmanagedCodeSecurity]
			internal delegate ulong glGetTextureHandleNV(uint texture);

			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[ThreadStatic]
			internal static glGetTextureHandleNV pglGetTextureHandleNV;

			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[SuppressUnmanagedCodeSecurity]
			internal delegate ulong glGetTextureSamplerHandleNV(uint texture, uint sampler);

			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[ThreadStatic]
			internal static glGetTextureSamplerHandleNV pglGetTextureSamplerHandleNV;

			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[SuppressUnmanagedCodeSecurity]
			internal delegate void glMakeTextureHandleResidentNV(ulong handle);

			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[ThreadStatic]
			internal static glMakeTextureHandleResidentNV pglMakeTextureHandleResidentNV;

			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[SuppressUnmanagedCodeSecurity]
			internal delegate void glMakeTextureHandleNonResidentNV(ulong handle);

			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[ThreadStatic]
			internal static glMakeTextureHandleNonResidentNV pglMakeTextureHandleNonResidentNV;

			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[SuppressUnmanagedCodeSecurity]
			internal delegate ulong glGetImageHandleNV(uint texture, int level, [MarshalAs(UnmanagedType.I1)] bool layered, int layer, int format);

			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[ThreadStatic]
			internal static glGetImageHandleNV pglGetImageHandleNV;

			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[SuppressUnmanagedCodeSecurity]
			internal delegate void glMakeImageHandleResidentNV(ulong handle, int access);

			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[ThreadStatic]
			internal static glMakeImageHandleResidentNV pglMakeImageHandleResidentNV;

			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[SuppressUnmanagedCodeSecurity]
			internal delegate void glMakeImageHandleNonResidentNV(ulong handle);

			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[ThreadStatic]
			internal static glMakeImageHandleNonResidentNV pglMakeImageHandleNonResidentNV;

			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[SuppressUnmanagedCodeSecurity]
			internal delegate void glUniformHandleui64NV(int location, ulong value);

			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[ThreadStatic]
			internal static glUniformHandleui64NV pglUniformHandleui64NV;

			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[SuppressUnmanagedCodeSecurity]
			internal delegate void glUniformHandleui64vNV(int location, int count, ulong* value);

			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[ThreadStatic]
			internal static glUniformHandleui64vNV pglUniformHandleui64vNV;

			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[SuppressUnmanagedCodeSecurity]
			internal delegate void glProgramUniformHandleui64NV(uint program, int location, ulong value);

			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[ThreadStatic]
			internal static glProgramUniformHandleui64NV pglProgramUniformHandleui64NV;

			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[SuppressUnmanagedCodeSecurity]
			internal delegate void glProgramUniformHandleui64vNV(uint program, int location, int count, ulong* values);

			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[ThreadStatic]
			internal static glProgramUniformHandleui64vNV pglProgramUniformHandleui64vNV;

			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[SuppressUnmanagedCodeSecurity]
			[return: MarshalAs(UnmanagedType.I1)]
			internal delegate bool glIsTextureHandleResidentNV(ulong handle);

			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[ThreadStatic]
			internal static glIsTextureHandleResidentNV pglIsTextureHandleResidentNV;

			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[SuppressUnmanagedCodeSecurity]
			[return: MarshalAs(UnmanagedType.I1)]
			internal delegate bool glIsImageHandleResidentNV(ulong handle);

			[RequiredByFeature("GL_NV_bindless_texture", Api = "gl|glcore|gles2")]
			[ThreadStatic]
			internal static glIsImageHandleResidentNV pglIsImageHandleResidentNV;

		}
	}

}
