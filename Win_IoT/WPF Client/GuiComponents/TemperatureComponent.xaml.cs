using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json.Linq;

namespace ABB.MagicMirror.GuiComponents
{
    public sealed partial class TemperatureComponent : UserControl
    {
        public string SensorId { get; set; }

        private DispatcherTimer timer;
      
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
                var temperatureMeasurementTask = SensorServiceWrapper.DownloadLatestMeasurementById(SensorId);
                JObject temperatureMeasurement = await temperatureMeasurementTask;
                if (temperatureMeasurement == null)
                {
                    return;
                }

                Value.Text =
                    temperatureMeasurement.First.Parent["data"]["value"].ToString() 
                    + temperatureMeasurement.First.Parent["data"]["unit"].ToString();

            }
            catch (Exception e)
            {
            }

        }
    }
}
