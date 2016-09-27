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
using System.Text;
using System.Threading.Tasks;

namespace Codeology.AWS
{
    public enum CompassDirection : byte
    {
        North = 0,
        NorthNorthEast = 1,
        NorthEast = 2,
        EastNorthEast = 3,
        East = 4,
        EastSouthEast = 5,
        SouthEast = 6,
        SouthSouthEast = 7,
        South = 8,
        SouthSouthWest = 9,
        SouthWest = 10,
        WestSouthWest = 11,
        West = 12,
        WestNorthWest = 13,
        NorthWest = 14,
        NorthNorthWest = 15
    }

    public enum AbbreviatedCompassDirection : byte
    {
        N = 0,
        NNE = 1,
        NE = 2,
        ENE = 3,
        E = 4,
        ESE = 5,
        SE = 6,
        SSE = 7,
        S = 8,
        SSW = 9,
        SW = 10,
        WSW = 11,
        W = 12,
        WNW = 13,
        NW = 14,
        NNW = 15
    }

    public struct Direction : IEquatable<Direction>
    {
        private ushort? value;

        public Direction(ushort direction)
        {
            value = direction;
        }

        public static bool operator ==(Direction left, Direction right)
        {
            return (left.Degrees == right.Degrees);
        }

        public static bool operator !=(Direction left, Direction right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return String.Format("{0} ({1}°)", AbbreviatedCompassDirection, Degrees);
        }

        public override int GetHashCode()
        {
            var hash = 27;

            unchecked
            {
                hash = (13 * hash) ^ value.GetHashCode();
            }

            return hash;
        }

        public override bool Equals(object obj)
        {
            return (obj is Direction && this == (Direction)obj);
        }

        public bool Equals(Direction direction)
        {
            return (this == direction);
        }

        public int Degrees
        {
            get
            {
                if (!value.HasValue)
                    value = 0;

                return value.Value;
            }
        }

        public CompassDirection CompassDirection
        {
            get
            {
                CompassDirection result;

                if ((Degrees >= 0 && Degrees < 11) || Degrees >= 348)
                {
                    result = CompassDirection.North;
                }
                else if (Degrees >= 11 && Degrees < 33)
                {
                    return CompassDirection.NorthNorthEast;
                }
                else if (Degrees >= 33 && Degrees < 56)
                {
                    result = CompassDirection.NorthEast;
                }
                else if (Degrees >= 56 && Degrees < 78)
                {
                    result = CompassDirection.EastNorthEast;
                }
                else if (Degrees >= 78 && Degrees < 101)
                {
                    result = CompassDirection.East;
                }
                else if (Degrees >= 101 && Degrees < 123)
                {
                    result = CompassDirection.EastSouthEast;
                }
                else if (Degrees >= 123 && Degrees < 146)
                {
                    result = CompassDirection.SouthEast;
                }
                else if (Degrees >= 146 && Degrees < 168)
                {
                    result = CompassDirection.SouthSouthEast;
                }
                else if (Degrees >= 168 && Degrees < 191)
                {
                    result = CompassDirection.South;
                }
                else if (Degrees >= 191 && Degrees < 213)
                {
                    result = CompassDirection.SouthSouthWest;
                }
                else if (Degrees >= 213 && Degrees < 236)
                {
                    result = CompassDirection.SouthWest;
                }
                else if (Degrees >= 236 && Degrees < 258)
                {
                    result = CompassDirection.WestSouthWest;
                }
                else if (Degrees >= 258 && Degrees < 281)
                {
                    result = CompassDirection.West;
                }
                else if (Degrees >= 281 && Degrees < 303)
                {
                    result = CompassDirection.WestNorthWest;
                }
                else if (Degrees >= 303 && Degrees < 326)
                {
                    result = CompassDirection.NorthWest;
                }
                else
                {
                    result = CompassDirection.NorthNorthWest;
                }

                return result;
            }
        }

        public AbbreviatedCompassDirection AbbreviatedCompassDirection
        {
            get
            {
                var direction = (byte)CompassDirection;

                return (AbbreviatedCompassDirection)direction;
            }
        }
    }
}
