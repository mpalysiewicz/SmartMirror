using ABB.Sensors.TemperatureWrapper;
using SensorDataForwarder;
using System;
using System.Collections.Generic;
using ABB.Sensors.ObjectModel;

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
            this.temperatureSensor = new TemperatureSensor(30000);
            if (temperatureSensor == null)
                return;
            temperatureSensor.TemperatureRead += TemperatureSensor_TemperatureRead;
        }

        private void TemperatureSensor_TemperatureRead(object sender, TemperatureReadingArgs e)
        {
            sensorDataSender.SendObjectAsJson(new SensorReading
            {
                name = "Temp sensor 1",
                id = "room1_temp",
                data = new List<Measurement>()
                     {
                        new Measurement {
                         measurement_time = DateTime.Now,
                         value = e.Temperature.ToString(),
                         unit = "°C",
                     },
                        new Measurement {
                         measurement_time = DateTime.Now,
                         value = e.Humidity.ToString(),
                         unit = "%",
                     }
                }
            });
        }
    }
}
