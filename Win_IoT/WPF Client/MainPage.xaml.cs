using System;
using Windows.UI.Xaml.Controls;
using ABB.Sensors.Motion;
using Windows.UI.Core;
using Windows.UI.Xaml;
using ABB.Sensors.Distance;

namespace ABB.MagicMirror
{
    public sealed partial class MainPage : Page
    {        
        private IMotionSensor motionSensor;
        private MotionDetectionResult motionDetectionResults;
        private class MotionDetectionResult
        {
            public DateTime From { get; set; }
            public DateTime To { get; set; }
        }

        private TemperatureSensorServiceWrapper.TemperatureSensor temperatureSensor;
        private DistanceSensorHCSR04 distanceSensor;
        private DispatcherTimer timer;

        public MainPage()
        {
            this.InitializeComponent();
            motionDetectionResults = new MotionDetectionResult();
            MotionStatus.DataContext = motionDetectionResults;

            motionSensor = MotionSensorFactory.Create();
            motionSensor.InitGPIO();
            motionSensor.MotionDetected += MotionSensor_MotionDetected;
            motionSensor.MotionUndetected += MotionSensor_MotionUndetected;

            temperatureSensor = new TemperatureSensorServiceWrapper.TemperatureSensor(10000);
            temperatureSensor.TemperatureRead += TemperatureSensor_TemperatureRead;


            distanceSensor = new DistanceSensorHCSR04(27, 22);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(5000);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private async void Timer_Tick(object sender, object e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var result = distanceSensor.Distance;
                distanceTbx.Text = result.ToString();
            });
        }

        private async void MotionSensor_MotionUndetected(IMotionSensor sender, string args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                motionDetectionResults.To = DateTime.Now;
            });            
        }

        private async void MotionSensor_MotionDetected(IMotionSensor sender, string e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                motionDetectionResults.From = DateTime.Now;
            });
        }

        private async void TemperatureSensor_TemperatureRead(object sender, TemperatureSensorServiceWrapper.TemperatureReadingArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                TempStatus.Text = e.Temperature + "°C /" + e.Humidity + "%";
            });
        }
    }
}