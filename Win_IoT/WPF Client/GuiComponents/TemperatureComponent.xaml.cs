using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json.Linq;

namespace ABB.MagicMirror.GuiComponents
{
    public sealed partial class TemperatureComponent : UserControl
    {
        public string Id { get; set; }
        private DispatcherTimer timer;
        private const string url = @"http://10.3.54.74:8082";

        public TemperatureComponent()
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
                var temperatureMeasurementTask = SensorServiceWrapper.DownloadLatestMeasurementById(url, "room1_temp");
                JObject temperatureMeasurement = await temperatureMeasurementTask;
                if (temperatureMeasurement == null)
                {
                    return;
                }

                TemperatureValue.Text =
                    temperatureMeasurement.First.Parent["data"]["value"].ToString() 
                    + temperatureMeasurement.First.Parent["data"]["unit"].ToString();

            }
            catch (Exception e)
            {
            }

        }
    }
}
