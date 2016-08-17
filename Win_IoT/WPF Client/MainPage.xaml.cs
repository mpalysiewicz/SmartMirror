using System;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MotionSensor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {        
        private MotionSensorService.MotionSensor motionSensor;
        private TemperatureSensorServiceWrapper.TemperatureSensor temperatureSensor;

        public MainPage()
        {
            this.InitializeComponent();

            motionSensor = new MotionSensorService.MotionSensor();
            motionSensor.InitGPIO();
            motionSensor.MotionDetected += MotionSensor_MotionDetected;
            motionSensor.MotionUndetected += MotionSensor_MotionUndetected;

            temperatureSensor = new TemperatureSensorServiceWrapper.TemperatureSensor(10000);
            temperatureSensor.TemperatureRead += TemperatureSensor_TemperatureRead;
        }

        private void MotionSensor_MotionUndetected(MotionSensorService.MotionSensor sender, string args)
        {
            GpioStatus.Items.Insert(0, "Motion undetected at " + DateTime.Now);
        }

        private void MotionSensor_MotionDetected(MotionSensorService.MotionSensor sender, string e)
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
