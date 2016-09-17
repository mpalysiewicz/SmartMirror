using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Core;
using Windows.UI.Xaml;
using ABB.Sensors.Distance;

namespace ABB.MagicMirror
{
    public sealed partial class MainPage : Page
    {        
        private DistanceSensorHCSR04 _distanceSensor;
        private DispatcherTimer timer;

        public MainPage()
        {
            this.InitializeComponent();

            ShowIpAddress();
                                    
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
            timer.Start();

            _distanceSensor = new DistanceSensorHCSR04();
            _distanceSensor.InitGPIO();
        }
                
        private void ShowIpAddress()
        {
            ipAddressTbx.Text = string.Join(",  ", Helpers.Networking.GetLocalIpAddress());
        }

        private async void Timer_Tick(object sender, object e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var distanceReading = _distanceSensor.Read();
                if(distanceReading == null)
                {
                    return;
                }

                distanceTbx.Text = distanceReading.DistanceInCm.ToString();
                SensorsDataSender.SendObjectAsJson(distanceReading);
            });
        }
        
       
    }
}