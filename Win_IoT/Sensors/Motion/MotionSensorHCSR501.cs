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
            if (currentValue == args.Edge) //TODO: Is it needed? the same edge should not happend twice, it is not state value but state change
                return;
            
            switch(args.Edge)
            {
                case GpioPinEdge.RisingEdge:
                    if (MotionDetected == null)
                        return;
                    MotionDetected.Invoke(this, null);
                    break;
                case GpioPinEdge.FallingEdge:
                    if (MotionUndetected == null)
                        return;                    
                    MotionUndetected.Invoke(this, null);
                    break;
            }

            currentValue = args.Edge;            
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