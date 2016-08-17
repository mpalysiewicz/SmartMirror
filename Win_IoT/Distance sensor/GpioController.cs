using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace //miejsce na namespace
{
    public class GpioController
    {
        public class Gpio
        {
            private static ManualResetEvent manualResetEvent = new ManualResetEvent(false);

            public static void Sleep(int delayMicroseconds)
            {
                manualResetEvent.WaitOne(
                    TimeSpan.FromMilliseconds((double)delayMicroseconds / 1000d));
            }

            private static Stopwatch stopWatch = new Stopwatch();

            public static double GetTimeUntilNextEdge(GpioPin pin, GpioPinValue edgeToWaitFor)
            {
                stopWatch.Reset();

                while (pin.Read() != edgeToWaitFor) { };

                stopWatch.Start();

                while (pin.Read() == edgeToWaitFor) { };

                stopWatch.Stop();

                return stopWatch.Elapsed.TotalSeconds;
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
        }
    }
}
