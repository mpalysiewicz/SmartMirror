using Windows.Devices.Gpio;

// TODO
// initialization:
//
//    _sensor = new HCSR04(27, 22);
//    var result = _sensor.Distance;


namespace ABB.Sensors.Distance
{
    public class DistanceSensorHCSR04
    {
        private GpioPin TriggerPin { get; set; }
        private GpioPin EchoPin { get; set; }
        private const double SPEED_OF_SOUND_METERS_PER_SECOND = 343;

        public DistanceSensorHCSR04(int triggerPin, int echoPin)
        {
            var controller = GpioController.GetDefault();

            TriggerPin = controller.OpenPin(triggerPin);
            TriggerPin.SetDriveMode(GpioPinDriveMode.Output);

            EchoPin = controller.OpenPin(echoPin);
            EchoPin.SetDriveMode(GpioPinDriveMode.Input);
        }

        private double LengthOfHighPulse
        {
            get
            {
                TriggerPin.Write(GpioPinValue.Low);
                Gpio.Sleep(50);
                TriggerPin.Write(GpioPinValue.High);
                Gpio.Sleep(100);
                TriggerPin.Write(GpioPinValue.Low);

                return Gpio.GetTimeUntilNextEdge(EchoPin, GpioPinValue.High, 1000);
            }
        }

        public double Distance
        {
            get
            {
                return (SPEED_OF_SOUND_METERS_PER_SECOND / 2) * LengthOfHighPulse;
            }
        }
    }
}