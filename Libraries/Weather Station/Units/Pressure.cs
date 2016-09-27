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
    public struct Pressure : IEquatable<Pressure>
    {
        private ushort? value;

        public Pressure(ushort tempValue)
        {
            value = tempValue;
        }

        public static bool operator ==(Pressure left, Pressure right)
        {
            return (left.Value == right.Value);
        }

        public static bool operator !=(Pressure left, Pressure right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return String.Format("{0} hPa / {1} psi / {2} inHg / {3} mmHg", Millibars, PoundsSquareInch, InchesMercury, MillimetresMercury);
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
            return (obj is Pressure && this == (Pressure)obj);
        }

        public bool Equals(Pressure pressure)
        {
            return (this == pressure);
        }

        public double Millibars
        {
            get
            {
                return (value == null ? 0.0 : value.Value / 10.0);
            }
        }

        public double PoundsSquareInch
        {
            get
            {
                var mb = (value == null ? 0.0 : value.Value / 10.0);

                return (0.014503773773 * mb);
            }
        }

        public double InchesMercury
        {
            get
            {
                var mb = (value == null ? 0.0 : value.Value / 10.0);

                return (0.0295300 * mb);
            }
        }

        public double MillimetresMercury
        {
            get
            {
                var mb = (value == null ? 0.0 : value.Value / 10.0);
                var inHg = (0.0295300 * mb);

                return (25.4 * inHg);
            }
        }

        private ushort Value
        {
            get
            {
                return value.Value;
            }
        }
    }
}
