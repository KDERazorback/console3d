
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
	public partial class Glx
	{
		/// <summary>
		/// [GLX] glXGetVideoSyncSGI: Binding for glXGetVideoSyncSGI.
		/// </summary>
		/// <param name="count">
		/// A <see cref="T:uint[]"/>.
		/// </param>
		[RequiredByFeature("GLX_SGI_video_sync")]
		public static int GetVideoSyncSGI([Out] uint[] count)
		{
			int retValue;

			unsafe {
				fixed (uint* p_count = count)
				{
					Debug.Assert(Delegates.pglXGetVideoSyncSGI != null, "pglXGetVideoSyncSGI not implemented");
					retValue = Delegates.pglXGetVideoSyncSGI(p_count);
					LogCommand("glXGetVideoSyncSGI", retValue, count					);
				}
			}
			DebugCheckErrors(retValue);

			return (retValue);
		}

		/// <summary>
		/// [GLX] glXWaitVideoSyncSGI: Binding for glXWaitVideoSyncSGI.
		/// </summary>
		/// <param name="divisor">
		/// A <see cref="T:int"/>.
		/// </param>
		/// <param name="remainder">
		/// A <see cref="T:int"/>.
		/// </param>
		/// <param name="count">
		/// A <see cref="T:uint[]"/>.
		/// </param>
		[RequiredByFeature("GLX_SGI_video_sync")]
		public static int WaitVideoSyncSGI(int divisor, int remainder, uint[] count)
		{
			int retValue;

			unsafe {
				fixed (uint* p_count = count)
				{
					Debug.Assert(Delegates.pglXWaitVideoSyncSGI != null, "pglXWaitVideoSyncSGI not implemented");
					retValue = Delegates.pglXWaitVideoSyncSGI(divisor, remainder, p_count);
					LogCommand("glXWaitVideoSyncSGI", retValue, divisor, remainder, count					);
				}
			}
			DebugCheckErrors(retValue);

			return (retValue);
		}

		internal static unsafe partial class Delegates
		{
			[RequiredByFeature("GLX_SGI_video_sync")]
			[SuppressUnmanagedCodeSecurity]
			internal delegate int glXGetVideoSyncSGI(uint* count);

			[RequiredByFeature("GLX_SGI_video_sync")]
			internal static glXGetVideoSyncSGI pglXGetVideoSyncSGI;

			[RequiredByFeature("GLX_SGI_video_sync")]
			[SuppressUnmanagedCodeSecurity]
			internal delegate int glXWaitVideoSyncSGI(int divisor, int remainder, uint* count);

			[RequiredByFeature("GLX_SGI_video_sync")]
			internal static glXWaitVideoSyncSGI pglXWaitVideoSyncSGI;

		}
	}

}