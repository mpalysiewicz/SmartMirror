using Newtonsoft.Json.Linq;
using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ABB.MagicMirror.GuiComponents
{
    public sealed partial class SecondRoomTemperatureComponent : UserControl
    {
        public string Id { get; set; }
        private DispatcherTimer timer;
        private const string url = @"http://10.3.55.17:8080";

        public SecondRoomTemperatureComponent()
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
                var temperatureMeasurementTask = SensorServiceWrapper.DownloadLatestMeasurementById(url, "room2_temp");
                JObject temperatureMeasurement = await temperatureMeasurementTask;
                if (temperatureMeasurement == null)
                {
                    return;
                }

                Room2TemperatureValue.Text =
                    temperatureMeasurement.First.Parent["data"]["value"].ToString()
                    + temperatureMeasurement.First.Parent["data"]["unit"].ToString();

            }
            catch (Exception e)
            {
            }

        }
    }
}
