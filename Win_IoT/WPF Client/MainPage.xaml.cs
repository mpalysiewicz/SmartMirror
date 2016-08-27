using System;
using Windows.UI.Xaml.Controls;
using ABB.Sensors.Motion;
using Windows.UI.Core;
using Windows.UI.Xaml;
using ABB.Sensors.Distance;
using System.Net.Sockets;
using Windows.Networking.Connectivity;
using Windows.Networking;
using System.Collections.Generic;
using System.ComponentModel;
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

        private TemperatureSensorServiceWrapper.TemperatureSensor temperatureSensor;        private TemperatureSensorServiceWrapper.TemperatureSensor temperatureSensor;
        private DistanceSensorHCSR04 distanceSensor;
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

            _distanceSensor = DistanceSensorFactory.Create();
            _distanceSensor.InitGPIO();
            _distanceSensor.DistanceChanged += DistanceSensor_DistanceChanged;

            //temporary disabled - CPU consumption to be checked...
            //distanceSensor = new DistanceSensorHCSR04(27, 22);
            //timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromMilliseconds(5000);
            //timer.Tick += Timer_Tick;
            //timer.Start();
        }

        private void ShowIpAddress()
        {
            ipAddressTbx.Text = string.Join(",  ", Helpers.Networking.GetLocalIpAddress());
        }

        private async void Timer_Tick(object sender, object e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var result = distanceSensor.Distance;
                distanceTbx.Text = result.ToString();
            });
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

        //private async void TemperatureSensor_TemperatureRead(object sender, TemperatureSensorServiceWrapper.TemperatureReadingArgs e)
        //{
        //    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
        //    {
        //        TempStatus.Text = e.Temperature + "°C /" + e.Humidity + "%";
        //    });
        //}
    }
}