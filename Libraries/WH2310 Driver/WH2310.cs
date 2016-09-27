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
    public static partial class WH2310
    {
        public const ushort VENDOR_ID   = 0x10C4;
        public const ushort PRODUCT_ID  = 0x8468;

        public const byte TIME_SYNC         = 0x01;
        public const byte READ_EEPROM       = 0x02;
        public const byte WRITE_EEPROM      = 0x03;
        public const byte READ_RECORD       = 0x04;
        public const byte READ_MAX          = 0x05;
        public const byte READ_MIN          = 0x06;
        public const byte READ_MAX_DAY      = 0x07;
        public const byte READ_MIN_DAY      = 0x08;
        public const byte CLEAR_MAX_MIN_DAY = 0x09;
        public const byte PARAM_CHANGED     = 0x0A;
        public const byte CLEAR_HISTORY     = 0x0B;
        public const byte READ_PARAM        = 0x0C;

        public const byte CMD_RESULT = 0xF0;

        public const ushort PARAM_ITEM_ALARM    = 0x0001;
        public const ushort PARAM_ITEM_TIMEZONE = 0x0002;
        public const ushort PARAM_ITEM_PARAM    = 0x0004;
        public const ushort PARAM_ITEM_MAX_MIN  = 0x0008;
        public const ushort PARAM_ITEM_HISTORY  = 0x0010;

        public const ushort RT_SUCCESS              = 0x0000;
        public const ushort RT_INVALID_USER_PASS    = 0x0001;
        public const ushort RT_INVALID_ID           = 0x0002;
        public const ushort RT_INVALID_CRC          = 0x0004;
        public const ushort RT_BUSY                 = 0x0008;
        public const ushort RT_TOO_SIZE             = 0x0010;
        public const ushort RT_ERROR                = 0x0020;
        public const ushort RT_UNKNOWN_CMD          = 0x0040;
        public const ushort RT_INVALID_PARAM        = 0x0080;

        public const byte INVALID_DATA_8    = 0xFF;
        public const ushort INVALID_DATA_16 = 0xFFFF;
        public const uint INVALID_DATA_32   = 0xFFFFFFFF;

        public static bool IsConnected()
        {
            var result = false;
            var handle = HIDAPI.hid_open(VENDOR_ID, PRODUCT_ID, IntPtr.Zero);

            if (handle.ToInt32() > 0)
            {
                try
                {
                    result = true;
                }
                finally
                {
                    HIDAPI.hid_close(handle);
                }
            }

            return result;
        }
    }
}
