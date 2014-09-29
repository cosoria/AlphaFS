﻿/* Copyright (c) 2008-2014 Peter Palotas, Alexandr Normuradov, Jeffrey Jangli
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy 
 *  of this software and associated documentation files (the "Software"), to deal 
 *  in the Software without restriction, including without limitation the rights 
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 *  copies of the Software, and to permit persons to whom the Software is 
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in 
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 *  THE SOFTWARE. 
 */

using System.Runtime.InteropServices;

namespace Alphaleonis.Win32.Filesystem
{
   internal static partial class NativeMethods
   {
      /// <summary>SP_DEVICE_INTERFACE_DETAIL_DATA - An SP_DEVICE_INTERFACE_DETAIL_DATA structure contains the path for a device interface.</summary>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct SpDeviceInterfaceDetailData
      {
         /// <summary>The size, in bytes, of the SP_DEVICE_INTERFACE_DETAIL_DATA structure.</summary>
         [MarshalAs(UnmanagedType.U4)] public uint cbSize;

         /// <summary>The device interface path. This path can be passed to Win32 functions such as CreateFile.</summary>
         [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MaxPath)] public readonly string DevicePath;
      }
   }
}