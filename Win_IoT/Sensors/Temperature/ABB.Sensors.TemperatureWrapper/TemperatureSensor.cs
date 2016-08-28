using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Gpio;


namespace ABB.Sensors.TemperatureWrapper
{
    public sealed class TemperatureSensor
    {
        private ABB.Sensors.Temperature.Dht22Sensor temperatureSensor;
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
                ABB.Sensors.Temperature.TemperatureSensorReading temperatureReading = GetTemperatureSensorReadingResult();

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

        private ABB.Sensors.Temperature.TemperatureSensorReading GetTemperatureSensorReadingResult()
        {
            ABB.Sensors.Temperature.TemperatureSensorReading temperatureReading = Task.Run(GetTemperatureSensorReading).Result;
            return temperatureReading;
        }

        private async Task<ABB.Sensors.Temperature.TemperatureSensorReading> GetTemperatureSensorReading()
        {
            return await temperatureSensor.GetReadingAsync().AsTask();          

        }

        private void InitGpio()
        {
            pin = GpioController.GetDefault().OpenPin(4, GpioSharingMode.Exclusive);
            temperatureSensor = new ABB.Sensors.Temperature.Dht22Sensor(pin, GpioPinDriveMode.Input);                        
        }


    }
}
