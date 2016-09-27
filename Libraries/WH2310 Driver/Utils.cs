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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Codeology.AWS.Drivers
{
    internal static class Utils
    {
        public static int SizeOf<T>() where T : struct
        {
            int result = Marshal.SizeOf(typeof(T));

            return result;
        }

        public static T Read<T>(byte[] bytes) where T : struct
        {
            IntPtr ptr = Marshal.AllocHGlobal(bytes.Length);

            try
            {
                Marshal.Copy(bytes, 0, ptr, bytes.Length);

                T result = (T)Marshal.PtrToStructure(ptr, typeof(T));

                return result;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public static T Read<T>(Stream stream) where T : struct
        {
            int size = SizeOf<T>();

            return Read<T>(stream, size);
        }

        public static T Read<T>(Stream stream, int size) where T : struct
        {
            return Read<T>(stream, size, false);
        }

        public static T Read<T>(Stream stream, int size, bool allowSmaller) where T : struct
        {
            byte[] buffer = new byte[size];
            int num_read = stream.Read(buffer, 0, buffer.Length);

            if (!allowSmaller && num_read < size)
                throw new IOException("Could not read all of structure from stream.");

            if (num_read < size)
                return default(T);

            return Read<T>(buffer);
        }

        public static byte ReadByte(Stream stream)
        {
            byte[] buffer = new byte[1];

            stream.Read(buffer, 0, buffer.Length);

            return buffer[0];
        }

        public static byte[] ReadBytes(Stream stream, int count)
        {
            byte[] buffer = new byte[count];

            stream.Read(buffer, 0, buffer.Length);

            return buffer;
        }

        public static ushort ReadUInt16(Stream stream)
        {
            byte[] buffer = new byte[sizeof(ushort)];

            stream.Read(buffer, 0, buffer.Length);

            return BitConverter.ToUInt16(buffer, 0);
        }

        public static uint ReadUInt32(Stream stream)
        {
            byte[] buffer = new byte[sizeof(uint)];

            stream.Read(buffer, 0, buffer.Length);

            return BitConverter.ToUInt32(buffer, 0);
        }

        public static byte[] ReverseBytes(byte[] buffer)
        {
            var result = new byte[buffer.Length];
            var stack = new Stack<byte>(buffer);

            for (var i = 0; i < buffer.Length; i++)
                result[i] = stack.Pop();

            return result;
        }

        internal static void SaveBytes(string fileName, byte[] buffer)
        {
            using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                file.Write(buffer, 0, buffer.Length);
                file.Flush();
            }
        }
    }
}
