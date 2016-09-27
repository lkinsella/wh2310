using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Codeology.AWS;
using Codeology.AWS.Drivers;

namespace Codeology.AWS.Demo
{
    public partial class MainForm : Form
    {
        private WH2310Driver driver;
        private IWeatherStation station;

        public MainForm()
        {
            InitializeComponent();
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            driver = new WH2310Driver();
            station = WeatherStation.Create(driver, false);

            station.Refresh();

            await Task.Delay(250);

            RefreshData();

            station.Start();
            timer.Start();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer.Stop();
            station.Stop();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm");
            lblDate.Text = DateTime.Now.ToString("M.dd");

            lblInTemp.Text = String.Format("{0}°C", station.IndoorTemperature.Celsius);
            lblInHumi.Text = String.Format("{0}%", station.IndoorHumidity);
            lblOutTemp.Text = String.Format("{0}°C", station.OutdoorTemperatre.Celsius);
            lblOutHumi.Text = String.Format("{0}%", station.OutdoorHumidity);
            lblPressure.Text = String.Format("{0} hPa", station.RelativePressure.Millibars);
            lblWind.Text = String.Format("{0} mph / {1}", station.GustSpeed.MilesPerHour.ToString("0.0"), station.WindDirection.AbbreviatedCompassDirection);
        }
    }
}
