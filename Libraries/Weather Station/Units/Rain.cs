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
    public struct Rain : IEquatable<Rain>
    {
        private double rate;
        private double hour;
        private double day;
        private double week;
        private double month;
        private double year;
        private double totals;

        public Rain(double rainRate, double rainHour, double rainDay, double rainWeek, double rainMonth, double rainYear, double rainTotals)
        {
            rate = rainRate;
            hour = rainHour;
            day = rainDay;
            week = rainWeek;
            month = rainMonth;
            year = rainYear;
            totals = rainTotals;
        }

        public static bool operator ==(Rain left, Rain right)
        {
            if (left.Rate != right.Rate)
                return false;

            if (left.Hour != right.Hour)
                return false;

            if (left.Day != right.Day)
                return false;

            if (left.Week != right.Week)
                return false;

            if (left.Month != right.Month)
                return false;

            if (left.Year != right.Year)
                return false;

            if (left.Totals != right.Totals)
                return false;

            return true;
        }

        public static bool operator !=(Rain left, Rain right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return String.Format("Rate: {0}mm / Hour: {1}mm / Day: {2}mm / Week: {3}mm / Month: {4}mm / Year: {5}mm / Totals: {6}mm", rate, hour, day, week, month, year, totals);
        }

        public override int GetHashCode()
        {
            var hash = 27;

            unchecked
            {
                hash = (13 * hash) ^ rate.GetHashCode();
                hash = (13 * hash) ^ hour.GetHashCode();
                hash = (13 * hash) ^ day.GetHashCode();
                hash = (13 * hash) ^ week.GetHashCode();
                hash = (13 * hash) ^ month.GetHashCode();
                hash = (13 * hash) ^ year.GetHashCode();
                hash = (13 * hash) ^ totals.GetHashCode();
            }

            return hash;
        }

        public override bool Equals(object obj)
        {
            return (obj is Rain && this == (Rain)obj);
        }

        public bool Equals(Rain rain)
        {
            return (this == rain);
        }

        public double Rate
        {
            get
            {
                return rate;
            }
        }

        public double Hour
        {
            get
            {
                return hour;
            }
        }

        public double Day
        {
            get
            {
                return day;
            }
        }

        public double Week
        {
            get
            {
                return week;
            }
        }

        public double Month
        {
            get
            {
                return month;
            }
        }

        public double Year
        {
            get
            {
                return year;
            }
        }

        public double Totals
        {
            get
            {
                return totals;
            }
        }
    }
}
