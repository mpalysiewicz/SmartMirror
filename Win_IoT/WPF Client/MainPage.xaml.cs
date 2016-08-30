using System;
using Windows.UI.Xaml.Controls;
using ABB.Sensors.Motion;
using Windows.UI.Core;
using Windows.UI.Xaml;
using ABB.Sensors.Distance;
using ABB.Sensors.TemperatureWrapper;
using TemperatureSensorServiceWrapper;

namespace ABB.MagicMirror
{
    public sealed partial class MainPage : Page
    {
        private IMotionSensor _motionSensor;
        private MotionDetectionResult motionDetectionResults;
        internal class MotionDetectionResult
        {
            public DateTime From { get; set; }
            public DateTime To { get; set; }
        }

        private TemperatureSensor temperatureSensor;
        private DistanceSensorHCSR04 _distanceSensor;
        private DispatcherTimer timer;

        public MainPage()
        {
            this.InitializeComponent();

            ShowIpAddress();

            motionDetectionResults = new MotionDetectionResult();
            MotionStatus.DataContext = motionDetectionResults;

            _motionSensor = MotionSensorFactory.Create();
            _motionSensor.InitGPIO();
            _motionSensor.MotionDetected += MotionSensor_MotionDetected;
            _motionSensor.MotionUndetected += MotionSensor_MotionUndetected;

            temperatureSensor = new TemperatureSensor(10000);
            temperatureSensor.TemperatureRead += TemperatureSensor_TemperatureRead;
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
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                currentDistance.Text = _distanceSensor.Read();
            });
        }

        private async void MotionSensor_MotionUndetected(IMotionSensor sender, string args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                MotionUnDectedTbx.Text = DateTime.Now.ToString();
                MotionLed.Off();
            });            
        }

        private async void MotionSensor_MotionDetected(IMotionSensor sender, string e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                MotionDectedTbx.Text = DateTime.Now.ToString();
                MotionLed.On();
            });
        }

        private async void TemperatureSensor_TemperatureRead(object sender, TemperatureReadingArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                TempStatus.Text = e.Temperature + "°C /" + e.Humidity + "%";
            });
        }
    }
}