using Newtonsoft.Json.Linq;
using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ABB.MagicMirror.GuiComponents
{
    public sealed partial class DustSensorComponent : UserControl
    {
        public string Id { get; set; }
        private DispatcherTimer timer;        

        public DustSensorComponent()
        {
            this.InitializeComponent();
            InitializeTimer();
        }

        public string SensorId { get; set; }

        public string Title { get { return this.TitleTextBox.Text; } set { this.TitleTextBox.Text = value; } }

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
                UpdateValue();
            });
        }

        private async void UpdateValue()
        {
            if (string.IsNullOrEmpty(SensorId))
                return;

            try
            {
                var measurement = SensorServiceWrapper.DownloadLatestMeasurementById(SensorId);
                JObject receivedData = await measurement;
                if (receivedData == null)
                {
                    return;
                }

                DustValue.Text = receivedData.First.Parent["data"]["value"].ToString() + receivedData.First.Parent["data"]["unit"].ToString();

            }
            catch (Exception e)
            {
            }

        }
    }
}
