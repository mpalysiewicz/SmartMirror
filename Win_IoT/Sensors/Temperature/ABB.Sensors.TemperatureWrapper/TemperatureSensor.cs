using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Gpio;


namespace ABB.Sensors.TemperatureWrapper
{
    public sealed class TemperatureSensor
    {
        private Temperature.Dht22Sensor temperatureSensor;
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
                Temperature.TemperatureSensorReading temperatureReading = GetTemperatureSensorReadingResult();

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

        private Temperature.TemperatureSensorReading GetTemperatureSensorReadingResult()
        {
            Temperature.TemperatureSensorReading temperatureReading = Task.Run(GetTemperatureSensorReading).Result;
            return temperatureReading;
        }

        private async Task<Temperature.TemperatureSensorReading> GetTemperatureSensorReading()
        {
            return await temperatureSensor.GetReadingAsync().AsTask();          

        }

        private void InitGpio()
        {
            var gpio = GpioController.GetDefault();
            if (gpio == null)
            {
                //There is no GPIO controller on this device.
                pin = null;
                return;
            }

            pin = gpio.OpenPin(4, GpioSharingMode.Exclusive);
            if (pin == null)
            {
                //There were problems initializing the GPIO pin.;
                return;
            }
            temperatureSensor = new Temperature.Dht22Sensor(pin, GpioPinDriveMode.Input);                        
        }


    }
}
