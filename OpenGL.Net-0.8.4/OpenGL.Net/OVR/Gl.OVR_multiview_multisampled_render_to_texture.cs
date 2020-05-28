
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
		/// [GL] glFramebufferTextureMultisampleMultiviewOVR: Binding for glFramebufferTextureMultisampleMultiviewOVR.
		/// </summary>
		/// <param name="target">
		/// A <see cref="T:FramebufferTarget"/>.
		/// </param>
		/// <param name="attachment">
		/// A <see cref="T:FramebufferAttachment"/>.
		/// </param>
		/// <param name="texture">
		/// A <see cref="T:uint"/>.
		/// </param>
		/// <param name="level">
		/// A <see cref="T:int"/>.
		/// </param>
		/// <param name="samples">
		/// A <see cref="T:int"/>.
		/// </param>
		/// <param name="baseViewIndex">
		/// A <see cref="T:int"/>.
		/// </param>
		/// <param name="numViews">
		/// A <see cref="T:int"/>.
		/// </param>
		[RequiredByFeature("GL_OVR_multiview_multisampled_render_to_texture", Api = "gles2")]
		public static void FramebufferTextureMultisampleMultiOVR(FramebufferTarget target, FramebufferAttachment attachment, uint texture, int level, int samples, int baseViewIndex, int numViews)
		{
			Debug.Assert(Delegates.pglFramebufferTextureMultisampleMultiviewOVR != null, "pglFramebufferTextureMultisampleMultiviewOVR not implemented");
			Delegates.pglFramebufferTextureMultisampleMultiviewOVR((int)target, (int)attachment, texture, level, samples, baseViewIndex, numViews);
			LogCommand("glFramebufferTextureMultisampleMultiviewOVR", null, target, attachment, texture, level, samples, baseViewIndex, numViews			);
			DebugCheckErrors(null);
		}

		internal static unsafe partial class Delegates
		{
			[RequiredByFeature("GL_OVR_multiview_multisampled_render_to_texture", Api = "gles2")]
			[SuppressUnmanagedCodeSecurity]
			internal delegate void glFramebufferTextureMultisampleMultiviewOVR(int target, int attachment, uint texture, int level, int samples, int baseViewIndex, int numViews);

			[RequiredByFeature("GL_OVR_multiview_multisampled_render_to_texture", Api = "gles2")]
			[ThreadStatic]
			internal static glFramebufferTextureMultisampleMultiviewOVR pglFramebufferTextureMultisampleMultiviewOVR;

		}
	}

}