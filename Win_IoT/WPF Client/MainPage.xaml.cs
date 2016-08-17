using System;
using Windows.UI.Xaml.Controls;
using ABB.Sensors.Motion;

namespace ABB.MagicMirror
{
    public sealed partial class MainPage : Page
    {        
        private IMotionSensor motionSensor;
        

        private TemperatureSensorServiceWrapper.TemperatureSensor temperatureSensor;

        public MainPage()
        {
            this.InitializeComponent();

            motionSensor = MotionSensorFactory.Create();
            motionSensor.InitGPIO();
            motionSensor.MotionDetected += MotionSensor_MotionDetected;
            motionSensor.MotionUndetected += MotionSensor_MotionUndetected;

            temperatureSensor = new TemperatureSensorServiceWrapper.TemperatureSensor(10000);
            temperatureSensor.TemperatureRead += TemperatureSensor_TemperatureRead;
        }

        private void MotionSensor_MotionUndetected(IMotionSensor sender, string args)
        {
            GpioStatus.Items.Insert(0, "Motion undetected at " + DateTime.Now);
        }

        private void MotionSensor_MotionDetected(IMotionSensor sender, string e)
        {
            GpioStatus.Items.Insert(0,"Motion detected at " + DateTime.Now);
        }

        private async void TemperatureSensor_TemperatureRead(object sender, TemperatureSensorServiceWrapper.TemperatureReadingArgs e)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                TempStatus.Text = e.Temperature + "°C /" + e.Humidity + "%";
            });
        }
    }
}