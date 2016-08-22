using System;
using Windows.UI.Xaml.Controls;
using ABB.Sensors.Motion;
using Windows.UI.Core;
using ABB.Sensors.Distance;
using Windows;

namespace ABB.MagicMirror
{
    public sealed partial class MainPage : Page
    {
        private IDistanceSensor _distanceSensor;
        private IMotionSensor _motionSensor;
        private MotionDetectionResult motionDetectionResults;
        internal class MotionDetectionResult
        {
            public DateTime From { get; set; }
            public DateTime To { get; set; }
        }

        public MainPage()
        {
            this.InitializeComponent();
            motionDetectionResults = new MotionDetectionResult();
            MotionStatus.DataContext = motionDetectionResults;

            _motionSensor = MotionSensorFactory.Create();
            _motionSensor.InitGPIO();
            _motionSensor.MotionDetected += MotionSensor_MotionDetected;
            _motionSensor.MotionUndetected += MotionSensor_MotionUndetected;

            _distanceSensor = DistanceSensorFactory.Create();
            _distanceSensor.InitGPIO();
            _distanceSensor.DistanceChanged += DistanceSensor_DistanceChanged;
        }

        private async void DistanceSensor_DistanceChanged(IDistanceSensor sender, string args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                currentDistance.Text = sender.Read();
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

        //private async void TemperatureSensor_TemperatureRead(object sender, TemperatureSensorServiceWrapper.TemperatureReadingArgs e)
        //{
        //    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
        //    {
        //        TempStatus.Text = e.Temperature + "°C /" + e.Humidity + "%";
        //    });
        //}
    }
}