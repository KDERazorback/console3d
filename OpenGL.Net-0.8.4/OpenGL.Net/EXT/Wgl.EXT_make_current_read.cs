
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
	public partial class Wgl
	{
		/// <summary>
		/// [WGL] wglMakeContextCurrentEXT: Binding for wglMakeContextCurrentEXT.
		/// </summary>
		/// <param name="hDrawDC">
		/// A <see cref="T:IntPtr"/>.
		/// </param>
		/// <param name="hReadDC">
		/// A <see cref="T:IntPtr"/>.
		/// </param>
		/// <param name="hglrc">
		/// A <see cref="T:IntPtr"/>.
		/// </param>
		[RequiredByFeature("WGL_EXT_make_current_read")]
		public static bool MakeContextCurrentEXT(IntPtr hDrawDC, IntPtr hReadDC, IntPtr hglrc)
		{
			bool retValue;

			Debug.Assert(Delegates.pwglMakeContextCurrentEXT != null, "pwglMakeContextCurrentEXT not implemented");
			retValue = Delegates.pwglMakeContextCurrentEXT(hDrawDC, hReadDC, hglrc);
			LogCommand("wglMakeContextCurrentEXT", retValue, hDrawDC, hReadDC, hglrc			);
			DebugCheckErrors(retValue);

			return (retValue);
		}

		/// <summary>
		/// [WGL] wglGetCurrentReadDCEXT: Binding for wglGetCurrentReadDCEXT.
		/// </summary>
		[RequiredByFeature("WGL_EXT_make_current_read")]
		public static IntPtr GetCurrentReadDCEXT()
		{
			IntPtr retValue;

			Debug.Assert(Delegates.pwglGetCurrentReadDCEXT != null, "pwglGetCurrentReadDCEXT not implemented");
			retValue = Delegates.pwglGetCurrentReadDCEXT();
			LogCommand("wglGetCurrentReadDCEXT", retValue			);
			DebugCheckErrors(retValue);

			return (retValue);
		}

		internal static unsafe partial class Delegates
		{
			[RequiredByFeature("WGL_EXT_make_current_read")]
			[SuppressUnmanagedCodeSecurity]
			internal delegate bool wglMakeContextCurrentEXT(IntPtr hDrawDC, IntPtr hReadDC, IntPtr hglrc);

			[RequiredByFeature("WGL_EXT_make_current_read")]
			internal static wglMakeContextCurrentEXT pwglMakeContextCurrentEXT;

			[RequiredByFeature("WGL_EXT_make_current_read")]
			[SuppressUnmanagedCodeSecurity]
			internal delegate IntPtr wglGetCurrentReadDCEXT();

			[RequiredByFeature("WGL_EXT_make_current_read")]
			internal static wglGetCurrentReadDCEXT pwglGetCurrentReadDCEXT;

		}
	}

}
