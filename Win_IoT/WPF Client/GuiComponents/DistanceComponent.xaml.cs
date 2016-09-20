using Newtonsoft.Json.Linq;
using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ABB.MagicMirror.GuiComponents
{
    public sealed partial class DistanceComponent : UserControl
    {
        public string Id { get; set; }
        private DispatcherTimer timer;
        private const string url = @"http://10.3.54.74:8082";

        public DistanceComponent()
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
                UpdateDistance();
            });
        }

        private async void UpdateDistance()
        {
            try
            {
                var distanceMeasurementTask = SensorServiceWrapper.DownloadLatestMeasurementById(url, "room1_dist");
                JObject distanceMeasurement = await distanceMeasurementTask;
                if (distanceMeasurement == null)
                {
                    return;
                }

                DistanceValue.Text = distanceMeasurement.First.Parent["data"]["value"].ToString() + distanceMeasurement.First.Parent["data"]["unit"].ToString();

            }
            catch (Exception e)
            {
            }

        }
    }
}
