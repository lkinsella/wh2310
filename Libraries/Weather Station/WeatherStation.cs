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
using System.Threading;
using System.Threading.Tasks;

using Codeology.AWS.Drivers;

namespace Codeology.AWS
{
    public interface IWeatherStation : IDisposable
    {
        void Start();
        void Stop();
        void Refresh();

        Temperature IndoorTemperature { get; }
        Temperature OutdoorTemperatre { get; }
        Temperature HeatIndex { get; }
        Temperature DewPoint { get; }

        int IndoorHumidity { get; }
        int OutdoorHumidity { get; }

        Pressure AbsolutePressure { get; }
        Pressure RelativePressure { get; }

        Temperature WindChill { get; }
        Direction WindDirection { get; }
        Speed WindSpeed { get; }
        Speed GustSpeed { get; }

        Rain Rain { get; }
    }

    public class WeatherStation : IWeatherStation
    {
        private object locker;
        private bool terminate;
        private Thread thread;
        private IWeatherStationDriver driver;

        private DateTime lastRefresh;
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

        private WeatherStation(IWeatherStationDriver awsDriver, bool autoStart)
        {
            locker = new object();
            terminate = false;
            thread = null;
            driver = awsDriver;

            if (autoStart)
                Start();
        }

        public static IWeatherStation Create(IWeatherStationDriver driver)
        {
            return Create(driver, true);
        }

        public static IWeatherStation Create(IWeatherStationDriver driver, bool autoStart)
        {
            return new WeatherStation(driver, autoStart);
        }

        public void Dispose()
        {
            Stop();
        }

        public void Start()
        {
            lock (locker)
            {
                if (thread != null)
                    return;

                terminate = false;

                thread = new Thread(ThreadProc);
                thread.IsBackground = true;
                thread.Start();
            }
        }

        public void Stop()
        {
            lock (locker)
            {
                if (thread == null)
                    return;

                terminate = true;
            }
        }

        public void Refresh()
        {
            var success = driver.Refresh();

            if (success)
            {
                lock (locker)
                {
                    lastRefresh = DateTime.UtcNow;

                    indoorTemp = driver.IndoorTemperature;
                    outdoorTemp = driver.OutdoorTemperatre;
                    heatIndex = driver.HeatIndex;
                    dewPoint = driver.DewPoint;
                    indoorHumi = driver.IndoorHumidity;
                    outdoorHumi = driver.OutdoorHumidity;
                    absPressure = driver.AbsolutePressure;
                    relPressure = driver.RelativePressure;
                    windChill = driver.WindChill;
                    windDirection = driver.WindDirection;
                    windSpeed = driver.WindSpeed;
                    gustSpeed = driver.GustSpeed;
                    rain = driver.Rain;
                }
            }
        }

        private void ThreadProc()
        {
            while (true)
            {
                lock (locker)
                {
                    if (terminate)
                        break;
                }

                Refresh();
                BurstSleep(10000);
            }

            lock (locker)
            {
                thread = null;
            }
        }

        private void BurstSleep(int timeout)
        {
            DateTime started = DateTime.UtcNow;

            while (true)
            {
                lock (locker)
                {
                    if (terminate)
                        break;
                }

                TimeSpan ts = DateTime.UtcNow - started;

                if (ts.TotalMilliseconds >= timeout)
                    break;

                Thread.Sleep(10);
            }
        }

        public DateTime LastRefresh
        {
            get
            {
                lock (locker)
                    return lastRefresh;
            }
        }

        public Temperature IndoorTemperature
        {
            get
            {
                lock (locker)
                    return indoorTemp;
            }
        }

        public Temperature OutdoorTemperatre
        {
            get
            {
                lock (locker)
                    return outdoorTemp;
            }
        }

        public Temperature HeatIndex
        {
            get
            {
                lock (locker)
                    return heatIndex;
            }
        }

        public Temperature DewPoint
        {
            get
            {
                lock (locker)
                    return dewPoint;
            }
        }

        public int IndoorHumidity
        {
            get
            {
                lock (locker)
                    return indoorHumi;
            }
        }

        public int OutdoorHumidity
        {
            get
            {
                lock (locker)
                    return outdoorHumi;
            }
        }

        public Pressure AbsolutePressure
        {
            get
            {
                lock (locker)
                    return absPressure;
            }
        }

        public Pressure RelativePressure
        {
            get
            {
                lock (locker)
                    return relPressure;
            }
        }

        public Temperature WindChill
        {
            get
            {
                lock (locker)
                    return windChill;
            }
        }

        public Direction WindDirection
        {
            get
            {
                lock (locker)
                    return windDirection;
            }
        }

        public Speed WindSpeed
        {
            get
            {
                lock (locker)
                    return windSpeed;
            }
        }

        public Speed GustSpeed
        {
            get
            {
                lock (locker)
                    return gustSpeed;
            }
        }

        public Rain Rain
        {
            get
            {
                lock (locker)
                    return rain;
            }
        }
    }
}
