using ABB.Sensors.TemperatureWrapper;
using SensorDataForwarder;

namespace Abb.Services.Temperature
{
    public sealed class TemperatureHumidityReader
    {
        private TemperatureSensor temperatureSensor;
        private SensorDataSender sensorDataSender;

        public TemperatureHumidityReader(string url)
        {
            sensorDataSender = new SensorDataSender(url);
        }

        private void InitializeTemperatureSensor()
        {
            this.temperatureSensor = new TemperatureSensor(5000);
            if (temperatureSensor == null)
                return;
            temperatureSensor.TemperatureRead += TemperatureSensor_TemperatureRead;
        }

        private void TemperatureSensor_TemperatureRead(object sender, TemperatureReadingArgs e)
        {
            sensorDataSender.SendObjectAsJson(e);
        }
    }
}
