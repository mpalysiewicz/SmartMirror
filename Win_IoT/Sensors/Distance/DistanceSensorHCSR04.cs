using System;
using Windows.Devices.Gpio;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

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
        private static Stopwatch stopWatch = new Stopwatch();
        private static ManualResetEvent manualResetEvent = new ManualResetEvent(false);

        public void InitGPIO()
        {
            var gpio = GpioController.GetDefault();
            if (gpio == null)
            {
                TriggerPin = null;
                EchoPin = null;
                return;
            }

            TriggerPin = gpio.OpenPin(27);
            EchoPin = gpio.OpenPin(22);
            if(!IsInitialized)
            {
                return;
            }

            TriggerPin.SetDriveMode(GpioPinDriveMode.Output);
            EchoPin.SetDriveMode(GpioPinDriveMode.Input);
        }

        private bool IsInitialized
        {
            get
            {
                return TriggerPin != null && EchoPin != null;
            }
        }

        public static void Sleep(int delayMicroseconds)
        {
            manualResetEvent.WaitOne(TimeSpan.FromMilliseconds(delayMicroseconds / 1000d));
        }

        public static double GetTimeUntilNextEdge(GpioPin pin, GpioPinValue edgeToWaitFor, int maximumTimeToWaitInMilliseconds)
        {
            var t = Task.Run(() =>
            {
                stopWatch.Reset();

                while (pin.Read() != edgeToWaitFor) { };

                stopWatch.Start();

                while (pin.Read() == edgeToWaitFor) { };

                stopWatch.Stop();

                return stopWatch.Elapsed.TotalSeconds;
            });

            var isCompleted = t.Wait(TimeSpan.FromMilliseconds(maximumTimeToWaitInMilliseconds));

            if (isCompleted)
            {
                return t.Result;
            }
            else
            {
                return 0.0d;
            }
        }

        private double GetLengthOfHighPulse()
        {
            TriggerPin.Write(GpioPinValue.Low);
            Sleep(5);
            TriggerPin.Write(GpioPinValue.High);
            Sleep(10);
            TriggerPin.Write(GpioPinValue.Low);

            return GetTimeUntilNextEdge(EchoPin, GpioPinValue.High, 100);
        }

        public string Read()
        {
            if (!IsInitialized)
            {
                return string.Empty;
            }

            var lengthOfHighPulse = GetLengthOfHighPulse();

            return Math.Round(17150 * lengthOfHighPulse, 2).ToString();
        }
    }
}