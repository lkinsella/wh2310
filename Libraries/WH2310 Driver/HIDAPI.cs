#region License
//  Copyright(c) 2016, Lloyd Kinsella
//  All rights reserved.
//  
//  Redistribution and use in source and binary forms, with or without
//  modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of Workshell Ltd nor the names of its contributors
//  may be used to endorse or promote products
//  derived from this software without specific prior written permission.
//  
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  DISCLAIMED.IN NO EVENT SHALL WORKSHELL BE LIABLE FOR ANY
//  DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//  (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//  ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Codeology.AWS.Drivers
{
    [Serializable]
    internal class HIDAPIException : Exception
    {
        public HIDAPIException() : base() { }

        public HIDAPIException(string message) : base(message) { }

        public HIDAPIException(string message, Exception innerException) : base(message, innerException) { }

        protected HIDAPIException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    internal class HIDAPI
    {
        private const string LIB_NAME = "hidapi.dll"; // Will need changing for Linux

        [StructLayout(LayoutKind.Sequential)]
        public struct HID_DEVICE_INFO
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string Path;
            public ushort VendorId;
            public ushort ProductId;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string SerialNumber;
            public ushort ReleaseNumber;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string ManufacturerString;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string ProductString;
            public ushort UsagePage;
            public ushort Usage;
            public uint InterfaceNumber;
            public IntPtr Next;
        }

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int hid_init();

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int hid_exit();

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr hid_open(ushort vendorId, ushort productId, IntPtr serialNumber);

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void hid_close(IntPtr device);

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr hid_enumerate(ushort vendorId, ushort productId);

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void hid_free_enumeration(IntPtr devices);

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int hid_write(IntPtr device, byte[] buffer, int length);

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int hid_read(IntPtr device, byte[] buffer, int length);

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int hid_send_feature_report(IntPtr device, byte[] buffer, int length);
    }
}
