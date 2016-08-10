using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace MotionSensorService
{
    public sealed class MotionSensor
    {
        GpioPin motionSensorPin;
        const int Pin_GPIO17 = 11;

        public void InitGPIO()
        {
            var gpio = GpioController.GetDefault();
                        
            if (gpio == null)
            {
                //There is no GPIO controller on this device.
                motionSensorPin = null;                
                return;
            }

            motionSensorPin = gpio.OpenPin(Pin_GPIO17);
                        
            if (motionSensorPin == null)
            {
                //There were problems initializing the GPIO pin.;
                return;
            }

            motionSensorPin.SetDriveMode(GpioPinDriveMode.Input);
            
            motionSensorPin.ValueChanged += MotionSensorPin_ValueChanged;
            //GPIO pin initialized correctly.
        }

        private void MotionSensorPin_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            if (args.Edge == GpioPinEdge.RisingEdge)

                MotionDetected.Invoke(this, null);
        }

        public event Windows.Foundation.TypedEventHandler<MotionSensor,string> MotionDetected;

        public string Read()
        {
            if (motionSensorPin == null)
                return string.Empty;
            return motionSensorPin.Read().ToString();
        }
    }
}