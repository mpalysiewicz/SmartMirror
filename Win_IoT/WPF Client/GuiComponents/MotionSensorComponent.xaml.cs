using ABB.Sensors.Motion;
using System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace ABB.MagicMirror.GuiComponents
{
    public sealed partial class MotionSensorComponent : UserControl
    {
        public MotionSensorComponent()
        {
            this.InitializeComponent();
            SetupMotionSensor();
        }
                
        private IMotionSensor motionSensor;
        private MotionDetectionResult motionDetectionResults;

        internal class MotionDetectionResult
        {
            public DateTime From { get; set; }
            public DateTime To { get; set; }
        }

        private void SetupMotionSensor()
        {
            motionDetectionResults = new MotionDetectionResult();
            MotionStatus.DataContext = motionDetectionResults;

            motionSensor = MotionSensorFactory.Create();
            motionSensor.InitGPIO();
            motionSensor.MotionDetected += MotionSensor_MotionDetected;
            motionSensor.MotionUndetected += MotionSensor_MotionUndetected;
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
    }
}
