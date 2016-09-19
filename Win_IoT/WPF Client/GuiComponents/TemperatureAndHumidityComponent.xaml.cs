using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json.Linq;

namespace ABB.MagicMirror.GuiComponents
{
    public sealed partial class TemperatureAndHumidityComponent : UserControl
    {
        public string Id { get; set; }
        private DispatcherTimer timer;

        public TemperatureAndHumidityComponent()
        {            
            this.InitializeComponent();
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private async void Timer_Tick(object sender, object e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                UpdateTemperature();
            });
        }

        private async void UpdateTemperature()
        {
            try
            {
                var temperatureMeasurementTask = SensorServiceWrapper.DownloadLatestMeasurementById(Id);
                JObject temperatureMeasurement = await temperatureMeasurementTask;
                if (temperatureMeasurement == null)
                {
                    return;
                }

                TemperatureValue.Text = temperatureMeasurement["data"][0]["value"].ToString() + temperatureMeasurement["data"][0]["unit"].ToString();// "°C";
                HumidiyValue.Text = temperatureMeasurement["data"][1]["value"].ToString() + temperatureMeasurement["data"][1]["unit"].ToString(); //temperatureMeasurement["Humidity"] + "%";

            }
            catch (Exception e)
            {
            }

        }
    }
}
