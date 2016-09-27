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

namespace Codeology.AWS.Drivers
{

    public sealed class WH2310Driver : IWeatherStationDriver
    {
        private Temperature indoorTemp;
        private Temperature outdoorTemp;
        private Temperature heatIndex;
        private Temperature dewPoint;
        private int indoorHumi;
        private int outdoorHumi;
        private Pressure absPressure;
        private Pressure relPressure;
        private Temperature windChill;
        private Direction windDirection;
        private Speed windSpeed;
        private Speed gustSpeed;
        private Rain rain;

        public WH2310Driver()
        {
            indoorTemp = new Temperature();
            outdoorTemp = new Temperature();
            heatIndex = new Temperature();
            dewPoint = new Temperature();
            indoorHumi = 0;
            outdoorHumi = 0;
            absPressure = new Pressure();
            relPressure = new Pressure();
            windChill = new Temperature();
            windDirection = new Direction();
            windSpeed = new Speed();
            gustSpeed = new Speed();
            rain = new Rain();
        }

        public bool Refresh()
        {
            var records = WH2310.ReadRecord();

            if (records == null || records.Length == 0)
                return false;

            WH2310.Record record = null;
            
            record = records.FirstOrDefault(r => r.Item == WH2310.ITEM_INTEMP);
            indoorTemp = (record == null ? new Temperature() : new Temperature((((WH2310.Record16)record).Value / 10.0) - 40.0));

            record = records.FirstOrDefault(r => r.Item == WH2310.ITEM_OUTTEMP);
            outdoorTemp = (record == null ? new Temperature() : new Temperature((((WH2310.Record16)record).Value / 10.0) - 40.0));

            record = records.FirstOrDefault(r => r.Item == WH2310.ITEM_HEATINDEX);
            heatIndex = (record == null ? new Temperature() : new Temperature((((WH2310.Record16)record).Value / 10.0) - 40.0));

            record = records.FirstOrDefault(r => r.Item == WH2310.ITEM_DEWPOINT);
            dewPoint = (record == null ? new Temperature() : new Temperature((((WH2310.Record16)record).Value / 10.0) - 40.0));

            record = records.FirstOrDefault(r => r.Item == WH2310.ITEM_INHUMI);
            indoorHumi = (record == null ? 0 : (((WH2310.Record8)record).Value));

            record = records.FirstOrDefault(r => r.Item == WH2310.ITEM_OUTHUMI);
            outdoorHumi = (record == null ? 0 : (((WH2310.Record8)record).Value));

            record = records.FirstOrDefault(r => r.Item == WH2310.ITEM_ABSBARO);
            absPressure = (record == null ? new Pressure() : new Pressure(((WH2310.Record16)record).Value));

            record = records.FirstOrDefault(r => r.Item == WH2310.ITEM_RELBARO);
            relPressure = (record == null ? new Pressure() : new Pressure(((WH2310.Record16)record).Value));

            record = records.FirstOrDefault(r => r.Item == WH2310.ITEM_WINDCHILL);
            windChill = (record == null ? new Temperature() : new Temperature((((WH2310.Record16)record).Value / 10.0) - 40.0));

            record = records.FirstOrDefault(r => r.Item == WH2310.ITEM_WINDDIRECTION);
            windDirection = (record == null ? new Direction() : new Direction(((WH2310.Record16)record).Value));

            record = records.FirstOrDefault(r => r.Item == WH2310.ITEM_WINDSPEED);
            windSpeed = (record == null ? new Speed() : new Speed(((WH2310.Record16)record).Value / 10.0));

            record = records.FirstOrDefault(r => r.Item == WH2310.ITEM_GUSTSPEED);
            gustSpeed = (record == null ? new Speed() : new Speed(((WH2310.Record16)record).Value / 10.0));

            record = records.FirstOrDefault(r => r.Item == WH2310.ITEM_RAINRATE);
            double rainRate = (record == null ? 0.0 : ((WH2310.Record32)record).Value / 10.0);

            record = records.FirstOrDefault(r => r.Item == WH2310.ITEM_RAINHOUR);
            double rainHour = (record == null ? 0.0 : ((WH2310.Record32)record).Value / 10.0);

            record = records.FirstOrDefault(r => r.Item == WH2310.ITEM_RAINDAY);
            double rainDay = (record == null ? 0.0 : ((WH2310.Record32)record).Value / 10.0);

            record = records.FirstOrDefault(r => r.Item == WH2310.ITEM_RAINWEEK);
            double rainWeek = (record == null ? 0.0 : ((WH2310.Record32)record).Value / 10.0);

            record = records.FirstOrDefault(r => r.Item == WH2310.ITEM_RAINMONTH);
            double rainMonth = (record == null ? 0.0 : ((WH2310.Record32)record).Value / 10.0);

            record = records.FirstOrDefault(r => r.Item == WH2310.ITEM_RAINYEAR);
            double rainYear = (record == null ? 0.0 : ((WH2310.Record32)record).Value / 10.0);

            record = records.FirstOrDefault(r => r.Item == WH2310.ITEM_RAINTOTALS);
            double rainTotals = (record == null ? 0.0 : ((WH2310.Record32)record).Value / 10.0);

            rain = new Rain(rainRate, rainHour, rainDay, rainWeek, rainMonth, rainYear, rainTotals);

            return true;
        }

        public Temperature IndoorTemperature
        {
            get
            {
                return indoorTemp;
            }
        }

        public Temperature OutdoorTemperatre
        {
            get
            {
                return outdoorTemp;
            }
        }

        public Temperature HeatIndex
        {
            get
            {
                return heatIndex;
            }
        }

        public Temperature DewPoint
        {
            get
            {
                return dewPoint;
            }
        }

        public int IndoorHumidity
        {
            get
            {
                return indoorHumi;
            }
        }

        public int OutdoorHumidity
        {
            get
            {
                return outdoorHumi;
            }
        }

        public Pressure AbsolutePressure
        {
            get
            {
                return absPressure;
            }
        }

        public Pressure RelativePressure
        {
            get
            {
                return relPressure;
            }
        }

        public Temperature WindChill
        {
            get
            {
                return windChill;
            }
        }

        public Direction WindDirection
        {
            get
            {
                return windDirection;
            }
        }

        public Speed WindSpeed
        {
            get
            {
                return windSpeed;
            }
        }

        public Speed GustSpeed
        {
            get
            {
                return gustSpeed;
            }
        }

        public Rain Rain
        {
            get
            {
                return rain;
            }
        }
    }

}
