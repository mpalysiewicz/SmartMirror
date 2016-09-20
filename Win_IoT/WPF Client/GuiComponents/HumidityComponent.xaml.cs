using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ABB.MagicMirror.GuiComponents
{
    public sealed partial class HumidityComponent : UserControl
    {
        private DispatcherTimer timer;

        public HumidityComponent()
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
                UpdateHumidity();
            });
        }

        private async void UpdateHumidity()
        {
            try
            {
                var humidityMeasurementTask = SensorServiceWrapper.DownloadLatestMeasurementById(SensorId);
                JObject humidityMeasurement = await humidityMeasurementTask;
                if (humidityMeasurement == null)
                {
                    return;
                }

                HumidiyValue.Text = 
                    humidityMeasurement.First.Parent["data"]["value"].ToString()
                    + humidityMeasurement.First.Parent["data"]["unit"].ToString(); 

            }
            catch (Exception e)
            {
            }
        }
    }
}
