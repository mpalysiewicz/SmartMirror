using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace TemperatureSensorServiceWrapper
{
    public sealed class TemperatureSensor
    {
        private TemperatureSensorService.Dht22Sensor temperatureSensor;
        private GpioPin pin;
        private Timer timer;

        public event EventHandler<TemperatureReadingArgs> TemperatureRead;

        public TemperatureSensor(int readIntervalInMilliseconds)
        {
            InitGpio();
            InitTimer(readIntervalInMilliseconds);
        }

        private void InitTimer(int tempReadRateInMilliseconds)
        {
            timer = new Timer(ReadTemperature, null, 0, tempReadRateInMilliseconds);
        }

        private void ReadTemperature(object state)
        {
            try
            {
                TemperatureSensorService.TemperatureSensorReading temperatureReading = GetTemperatureSensorReadingResult();

                if (temperatureReading.IsValid && TemperatureRead != null)
                {
                        TemperatureRead.Invoke(this, new TemperatureReadingArgs(temperatureReading.Temperature, temperatureReading.Humidity));                                
                }
            }
            catch(Exception e)
            {
                return;
            }
        }

        private TemperatureSensorService.TemperatureSensorReading GetTemperatureSensorReadingResult()
        {
            TemperatureSensorService.TemperatureSensorReading temperatureReading = Task.Run(GetTemperatureSensorReading).Result;
            return temperatureReading;
        }

        private async Task<TemperatureSensorService.TemperatureSensorReading> GetTemperatureSensorReading()
        {
            return await temperatureSensor.GetReadingAsync().AsTask();          

        }

        private void InitGpio()
        {
            pin = GpioController.GetDefault().OpenPin(4, GpioSharingMode.Exclusive);
            temperatureSensor = new TemperatureSensorService.Dht22Sensor(pin, GpioPinDriveMode.Input);                        
        }


    }
}
