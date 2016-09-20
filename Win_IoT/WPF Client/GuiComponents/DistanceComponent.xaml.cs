using Newtonsoft.Json.Linq;
using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ABB.MagicMirror.GuiComponents
{
    public sealed partial class DistanceComponent : UserControl
    {
        public string Id { get; set; }
        private DispatcherTimer timer;       

        public DistanceComponent()
        {
            this.InitializeComponent();
            InitializeTimer();
        }

        public string SensorId { get; set; }

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
                UpdateDistance();
            });
        }

        private async void UpdateDistance()
        {
            try
            {
                var distanceMeasurementTask = SensorServiceWrapper.DownloadLatestMeasurementById(SensorId);
                JObject distanceMeasurement = await distanceMeasurementTask;
                if (distanceMeasurement == null)
                {
                    return;
                }

                Value.Text = distanceMeasurement.First.Parent["data"]["value"].ToString() + distanceMeasurement.First.Parent["data"]["unit"].ToString();

            }
            catch (Exception e)
            {
            }

        }
    }
}
