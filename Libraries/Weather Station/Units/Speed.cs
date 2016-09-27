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
    public struct Speed : IEquatable<Speed>
    {
        private double? value;

        public Speed(double metresSec)
        {
            value = metresSec;
        }

        public static bool operator ==(Speed left, Speed right)
        {
            return (left.MetresSec == right.MetresSec);
        }

        public static bool operator !=(Speed left, Speed right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return String.Format("{0} m/s / {1} mph / {2} kph / {3} knots", MetresSec, MilesPerHour, KilometresPerHour, Knots);
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
            return (obj is Speed && this == (Speed)obj);
        }

        public bool Equals(Speed speed)
        {
            return (this == speed);
        }

        public double MetresSec
        {
            get
            {
                if (!value.HasValue)
                    value = 0.0;

                return (value == null ? 0 : value.Value);
            }
        }

        public double MilesPerHour
        {
            get
            {
                return (value == null ? 0.0 : MetresSec * 2.237);
            }
        }

        public double KilometresPerHour
        {
            get
            {
                return (value == null ? 0.0 : MetresSec * 3.6);
            }
        }

        public double Knots
        {
            get
            {
                return (value == null ? 0.0 : MetresSec * 1.94384);
            }
        }
    }
}
