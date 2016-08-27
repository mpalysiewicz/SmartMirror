using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace ABB.Sensors.Motion
{
    public sealed class MotionSensorHCSR501 : IMotionSensor
    {
        GpioPin motionSensorPin;
        const int Pin_GPIO26 = 26;

        public void InitGPIO()
        {
            var gpio = GpioController.GetDefault();
                        
            if (gpio == null)
            {
                //There is no GPIO controller on this device.
                motionSensorPin = null;                
                return;
            }

            motionSensorPin = gpio.OpenPin(Pin_GPIO26, GpioSharingMode.Exclusive);
                        
            if (motionSensorPin == null)
            {
                //There were problems initializing the GPIO pin.;
                return;
            }

            //motionSensorPin.DebounceTimeout = System.TimeSpan.FromMilliseconds(50);
            motionSensorPin.SetDriveMode(GpioPinDriveMode.Input);
            
            motionSensorPin.ValueChanged += MotionSensorPin_ValueChanged;

            //Calibration time
            Task.Delay(10).Wait();
            Read();
            //GPIO pin initialized correctly.
        }

        GpioPinEdge currentValue = GpioPinEdge.FallingEdge;

        private void MotionSensorPin_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            if (args.Edge == GpioPinEdge.RisingEdge && currentValue != GpioPinEdge.RisingEdge && MotionDetected != null)
                MotionDetected.Invoke(this, null);
            else if (currentValue != GpioPinEdge.FallingEdge && MotionUndetected != null)
                MotionUndetected.Invoke(this, null);

            currentValue = args.Edge;
            //Task.Delay(5000).Wait();
        }

        public event Windows.Foundation.TypedEventHandler<IMotionSensor, string> MotionDetected;
        public event Windows.Foundation.TypedEventHandler<IMotionSensor, string> MotionUndetected;

        public string Read()
        {
            if (motionSensorPin == null)
                return string.Empty;
            
            return motionSensorPin.Read().ToString();
        }
    }
}