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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Codeology.AWS.Drivers
{
    static partial class WH2310
    {
        public const byte ITEM_INTEMP = 0x01;
        public const byte ITEM_OUTTEMP = 0x02;
        public const byte ITEM_DEWPOINT = 0x03;
        public const byte ITEM_WINDCHILL = 0x04;
        public const byte ITEM_HEATINDEX = 0x05;
        public const byte ITEM_INHUMI = 0x06;
        public const byte ITEM_OUTHUMI = 0x07;
        public const byte ITEM_ABSBARO = 0x08;
        public const byte ITEM_RELBARO = 0x09;
        public const byte ITEM_WINDDIRECTION = 0x0A;
        public const byte ITEM_WINDSPEED = 0x0B;
        public const byte ITEM_GUSTSPEED = 0x0C;
        public const byte ITEM_RAINEVENT = 0x0D;
        public const byte ITEM_RAINRATE = 0x0E;
        public const byte ITEM_RAINHOUR = 0x0F;
        public const byte ITEM_RAINDAY = 0x10;
        public const byte ITEM_RAINWEEK = 0x11;
        public const byte ITEM_RAINMONTH = 0x12;
        public const byte ITEM_RAINYEAR = 0x13;
        public const byte ITEM_RAINTOTALS = 0x14;
        public const byte ITEM_LIGHT = 0x15;
        public const byte ITEM_UV = 0x16;
        public const byte ITEM_UVI = 0x17;

        public const byte ITEM_TIME = 0x40;
        public const byte ITEM_DATE = 0x80;

        public sealed class RecordDate
        {
            internal RecordDate(byte year, byte month, byte day)
            {
                Year = year;
                Month = month;
                Day = day;
            }

            public override string ToString()
            {
                return String.Format("20{0:D2}-{1:D2}-{2:D2}", Year, Month, Day);
            }

            public byte Year { get; private set; }
            public byte Month { get; private set; }
            public byte Day { get; private set; }
        }

        public sealed class RecordTime
        {
            internal RecordTime(byte hour, byte minute)
            {
                Hour = hour;
                Minute = minute;
            }

            public override string ToString()
            {
                return String.Format("{0:D2}:{1:D2}", Hour, Minute);
            }

            public byte Hour { get; private set; }
            public byte Minute { get; private set; }
        }

        public abstract class Record
        {
            internal Record(byte item, RecordDate date, RecordTime time)
            {
                Item = item;
                Date = date;
                Time = time;
            }

            public override string ToString()
            {
                var result = base.ToString();

                switch (Item)
                {
                    case ITEM_INTEMP:
                        result = "Indoor Temperature";
                        break;
                    case ITEM_OUTTEMP:
                        result = "Outdoor Temperature";
                        break;
                    case ITEM_DEWPOINT:
                        result = "Dew Point";
                        break;
                    case ITEM_WINDCHILL:
                        result = "Wind Chill";
                        break;
                    case ITEM_HEATINDEX:
                        result = "Heat Index";
                        break;
                    case ITEM_INHUMI:
                        result = "Indoor Humidity";
                        break;
                    case ITEM_OUTHUMI:
                        result = "Outdoor Humidity";
                        break;
                    case ITEM_ABSBARO:
                        result = "Absolute Pressure";
                        break;
                    case ITEM_RELBARO:
                        result = "Relative Pressure";
                        break;
                    case ITEM_WINDDIRECTION:
                        result = "Wind Direction";
                        break;
                    case ITEM_WINDSPEED:
                        result = "Wind Speed";
                        break;
                    case ITEM_GUSTSPEED:
                        result = "Gust Speed";
                        break;
                    case ITEM_RAINEVENT:
                        result = "Rain Event";
                        break;
                    case ITEM_RAINRATE:
                        result = "Rain Rate";
                        break;
                    case ITEM_RAINHOUR:
                        result = "Rain Rate (Hourly)";
                        break;
                    case ITEM_RAINDAY:
                        result = "Rain Rate (Daily)";
                        break;
                    case ITEM_RAINWEEK:
                        result = "Rain Rate (Weekly)";
                        break;
                    case ITEM_RAINMONTH:
                        result = "Rain Rate (Monthly)";
                        break;
                    case ITEM_RAINYEAR:
                        result = "Rain Rate (Yearly)";
                        break;
                    case ITEM_RAINTOTALS:
                        result = "Rain Rate (Season)";
                        break;
                    case ITEM_LIGHT:
                        result = "Light";
                        break;
                    case ITEM_UV:
                        result = "UV";
                        break;
                    case ITEM_UVI:
                        result = "UV Index";
                        break;
                }

                return result;
            }

            public byte Item { get; private set; }
            public RecordDate Date { get; private set; }
            public RecordTime Time { get; private set; }
        }

        public sealed class Record8 : Record
        {
            internal Record8(byte item, RecordDate date, RecordTime time, byte value) : base(item, date, time)
            {
                Value = value;
            }

            public override string ToString()
            {
                return String.Format("{0}: {1}", base.ToString(), Value);
            }

            public byte Value { get; private set; }
        }

        public sealed class Record16 : Record
        {
            internal Record16(byte item, RecordDate date, RecordTime time, ushort value) : base(item, date, time)
            {
                Value = value;
            }

            public override string ToString()
            {
                return String.Format("{0}: {1}", base.ToString(), Value);
            }

            public ushort Value { get; private set; }
        }

        public sealed class Record32 : Record
        {
            internal Record32(byte item, RecordDate date, RecordTime time, uint value) : base(item, date, time)
            {
                Value = value;
            }

            public override string ToString()
            {
                return String.Format("{0}: {1}", base.ToString(), Value);
            }

            public uint Value { get; private set; }
        }

        public static Record[] ReadRecord()
        {
            var records = new List<Record>();
            var handle = HIDAPI.hid_open(VENDOR_ID, PRODUCT_ID, IntPtr.Zero);

            if (handle.ToInt32() <= 0)
            {
                Debug.Print("Could not open device.");

                return null;
            }

            try
            {
                var outBuffer = new byte[]
                {
                    0x02,           // USB Read Report
                    0x02,           // Size of payload
                    READ_RECORD,    // READ_RECORD
                    READ_RECORD     // Checksum (only one byte so same as command)
                };

                var numWritten = HIDAPI.hid_write(handle, outBuffer, outBuffer.Length);

                if (numWritten < 4)
                {
                    Debug.Print("Failed to write request to device.");

                    return null;
                }

                using (var mem = new MemoryStream(1024))
                {
                    var payload = PerformRead(handle);

                    if (payload == null)
                    {
                        Debug.Print("Could not read data from device.");

                        return null;
                    }

                    if (payload[0] != READ_RECORD)
                    {
                        Debug.Print("Payload is not a READ_RECORD result.");

                        return null;
                    }

                    var totalSize = payload[1];
                    var data = new byte[payload.Length - 2];

                    Array.Copy(payload, 2, data, 0, data.Length);

                    mem.Write(data, 0, data.Length);

                    while (mem.Length < totalSize)
                    {
                        data = PerformRead(handle);

                        if (data == null)
                        {
                            Debug.Print("Could not read data from device.");

                            return null;
                        }

                        if ((mem.Length + data.Length) >= totalSize)
                        {
                            mem.Write(data, 0, data.Length - 1); // -1 because we don't want to include the final checksum byte
                        }
                        else
                        {
                            mem.Write(data, 0, data.Length);
                        }
                    }

                    mem.Seek(0, SeekOrigin.Begin);

                    while (mem.Position < mem.Length)
                    {
                        var type = Utils.ReadByte(mem);
                        var item = type;
                        var hasDate = (type & ITEM_DATE) != 0;
                        var hasTime = (type & ITEM_TIME) != 0;

                        if (hasDate)
                            item = Convert.ToByte(item & ~ITEM_DATE);

                        if (hasTime)
                            item = Convert.ToByte(item & ~ITEM_TIME);

                        var valueBytes = new byte[0];

                        switch (item)
                        {
                            case ITEM_INTEMP:
                                valueBytes = Utils.ReadBytes(mem, 2);
                                break;
                            case ITEM_OUTTEMP:
                                valueBytes = Utils.ReadBytes(mem, 2);
                                break;
                            case ITEM_DEWPOINT:
                                valueBytes = Utils.ReadBytes(mem, 2);
                                break;
                            case ITEM_WINDCHILL:
                                valueBytes = Utils.ReadBytes(mem, 2);
                                break;
                            case ITEM_HEATINDEX:
                                valueBytes = Utils.ReadBytes(mem, 2);
                                break;
                            case ITEM_INHUMI:
                                valueBytes = Utils.ReadBytes(mem, 1);
                                break;
                            case ITEM_OUTHUMI:
                                valueBytes = Utils.ReadBytes(mem, 1);
                                break;
                            case ITEM_ABSBARO:
                                valueBytes = Utils.ReadBytes(mem, 2);
                                break;
                            case ITEM_RELBARO:
                                valueBytes = Utils.ReadBytes(mem, 2);
                                break;
                            case ITEM_WINDDIRECTION:
                                valueBytes = Utils.ReadBytes(mem, 2);
                                break;
                            case ITEM_WINDSPEED:
                                valueBytes = Utils.ReadBytes(mem, 2);
                                break;
                            case ITEM_GUSTSPEED:
                                valueBytes = Utils.ReadBytes(mem, 2);
                                break;
                            case ITEM_RAINEVENT:
                                valueBytes = Utils.ReadBytes(mem, 4);
                                break;
                            case ITEM_RAINRATE:
                                valueBytes = Utils.ReadBytes(mem, 4);
                                break;
                            case ITEM_RAINHOUR:
                                valueBytes = Utils.ReadBytes(mem, 4);
                                break;
                            case ITEM_RAINDAY:
                                valueBytes = Utils.ReadBytes(mem, 4);
                                break;
                            case ITEM_RAINWEEK:
                                valueBytes = Utils.ReadBytes(mem, 4);
                                break;
                            case ITEM_RAINMONTH:
                                valueBytes = Utils.ReadBytes(mem, 4);
                                break;
                            case ITEM_RAINYEAR:
                                valueBytes = Utils.ReadBytes(mem, 4);
                                break;
                            case ITEM_RAINTOTALS:
                                valueBytes = Utils.ReadBytes(mem, 4);
                                break;
                            case ITEM_LIGHT:
                                valueBytes = Utils.ReadBytes(mem, 4);
                                break;
                            case ITEM_UV:
                                valueBytes = Utils.ReadBytes(mem, 2);
                                break;
                            case ITEM_UVI:
                                valueBytes = Utils.ReadBytes(mem, 1);
                                break;
                            default:
                                return null;
                        }

                        RecordDate date = null;
                        RecordTime time = null;

                        if (hasDate)
                        {
                            var dateBuffer = new byte[3];
                            var numRead = mem.Read(dateBuffer, 0, dateBuffer.Length);

                            if (numRead < dateBuffer.Length)
                            {
                                Debug.Print("Could not read date from memory.");

                                return null;
                            }

                            date = new RecordDate(dateBuffer[0], dateBuffer[1], dateBuffer[2]);
                        }

                        if (hasTime)
                        {
                            var timeBuffer = new byte[2];
                            var numRead = mem.Read(timeBuffer, 0, timeBuffer.Length);

                            if (numRead < timeBuffer.Length)
                            {
                                Debug.Print("Could not read time from memory.");

                                return null;
                            }

                            time = new RecordTime(timeBuffer[0], timeBuffer[1]);
                        }

                        Record record = null;

                        switch (item)
                        {
                            case ITEM_INTEMP:
                                {
                                    ushort value = BitConverter.ToUInt16(Utils.ReverseBytes(valueBytes), 0);

                                    record = new Record16(item, date, time, value);

                                    break;
                                }
                            case ITEM_OUTTEMP:
                                {
                                    ushort value = BitConverter.ToUInt16(Utils.ReverseBytes(valueBytes), 0);

                                    record = new Record16(item, date, time, value);

                                    break;
                                }
                            case ITEM_DEWPOINT:
                                {
                                    ushort value = BitConverter.ToUInt16(Utils.ReverseBytes(valueBytes), 0);

                                    record = new Record16(item, date, time, value);

                                    break;
                                }
                            case ITEM_WINDCHILL:
                                {
                                    ushort value = BitConverter.ToUInt16(Utils.ReverseBytes(valueBytes), 0);

                                    record = new Record16(item, date, time, value);

                                    break;
                                }
                            case ITEM_HEATINDEX:
                                {
                                    ushort value = BitConverter.ToUInt16(Utils.ReverseBytes(valueBytes), 0);

                                    record = new Record16(item, date, time, value);

                                    break;
                                }
                            case ITEM_INHUMI:
                                {
                                    record = new Record8(item, date, time, valueBytes[0]);

                                    break;
                                }
                            case ITEM_OUTHUMI:
                                {
                                    record = new Record8(item, date, time, valueBytes[0]);

                                    break;
                                }
                            case ITEM_ABSBARO:
                                {
                                    ushort value = BitConverter.ToUInt16(Utils.ReverseBytes(valueBytes), 0);

                                    record = new Record16(item, date, time, value);

                                    break;
                                }
                            case ITEM_RELBARO:
                                {
                                    ushort value = BitConverter.ToUInt16(Utils.ReverseBytes(valueBytes), 0);

                                    record = new Record16(item, date, time, value);

                                    break;
                                }
                            case ITEM_WINDDIRECTION:
                                {
                                    ushort value = BitConverter.ToUInt16(Utils.ReverseBytes(valueBytes), 0);

                                    record = new Record16(item, date, time, value);

                                    break;
                                }
                            case ITEM_WINDSPEED:
                                {
                                    ushort value = BitConverter.ToUInt16(Utils.ReverseBytes(valueBytes), 0);

                                    record = new Record16(item, date, time, value);

                                    break;
                                }
                            case ITEM_GUSTSPEED:
                                {
                                    ushort value = BitConverter.ToUInt16(Utils.ReverseBytes(valueBytes), 0);

                                    record = new Record16(item, date, time, value);

                                    break;
                                }
                            case ITEM_RAINEVENT:
                                {
                                    uint value = BitConverter.ToUInt32(Utils.ReverseBytes(valueBytes), 0);

                                    record = new Record32(item, date, time, value);

                                    break;
                                }
                            case ITEM_RAINRATE:
                                {
                                    uint value = BitConverter.ToUInt32(Utils.ReverseBytes(valueBytes), 0);

                                    record = new Record32(item, date, time, value);

                                    break;
                                }
                            case ITEM_RAINHOUR:
                                {
                                    uint value = BitConverter.ToUInt32(Utils.ReverseBytes(valueBytes), 0);

                                    record = new Record32(item, date, time, value);

                                    break;
                                }
                            case ITEM_RAINDAY:
                                {
                                    uint value = BitConverter.ToUInt32(Utils.ReverseBytes(valueBytes), 0);

                                    record = new Record32(item, date, time, value);

                                    break;
                                }
                            case ITEM_RAINWEEK:
                                {
                                    uint value = BitConverter.ToUInt32(Utils.ReverseBytes(valueBytes), 0);

                                    record = new Record32(item, date, time, value);

                                    break;
                                }
                            case ITEM_RAINMONTH:
                                {
                                    uint value = BitConverter.ToUInt32(Utils.ReverseBytes(valueBytes), 0);

                                    record = new Record32(item, date, time, value);

                                    break;
                                }
                            case ITEM_RAINYEAR:
                                {
                                    uint value = BitConverter.ToUInt32(Utils.ReverseBytes(valueBytes), 0);

                                    record = new Record32(item, date, time, value);

                                    break;
                                }
                            case ITEM_RAINTOTALS:
                                {
                                    uint value = BitConverter.ToUInt32(Utils.ReverseBytes(valueBytes), 0);

                                    record = new Record32(item, date, time, value);

                                    break;
                                }
                            case ITEM_LIGHT:
                                {
                                    uint value = BitConverter.ToUInt32(Utils.ReverseBytes(valueBytes), 0);

                                    record = new Record32(item, date, time, value);

                                    break;
                                }
                            case ITEM_UV:
                                {
                                    ushort value = BitConverter.ToUInt16(Utils.ReverseBytes(valueBytes), 0);

                                    record = new Record16(item, date, time, value);

                                    break;
                                }
                            case ITEM_UVI:
                                {
                                    record = new Record8(item, date, time, valueBytes[0]);

                                    break;
                                }
                        }

                        if (record != null)
                            records.Add(record);
                    }
                }
            }
            finally
            {
                HIDAPI.hid_close(handle);
            }

            return records.ToArray();
        }

        private static byte[] PerformRead(IntPtr handle)
        {
            var inBuffer = new byte[61];
            var numRead = HIDAPI.hid_read(handle, inBuffer, inBuffer.Length);

            if (numRead == 0)
                return null;

            if (inBuffer[0] != 0x01)
                return null;

            var payloadSize = inBuffer[1];
            var payload = new byte[payloadSize];

            Array.Copy(inBuffer, 2, payload, 0, payloadSize);

            return payload;
        }
    }
}
