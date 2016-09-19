using ABB.Sensors.ObjectModel;
using ABB.Sensors.Distance;
using SensorDataForwarder;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DistanceSensorService
{
    class DistanceSensorReader
    {
        private DistanceSensorHCSR04 _distanceSensor;
        private Timer timer;
        private SensorDataSender sensorDataSender;

        public DistanceSensorReader(string url)
        {
            sensorDataSender = new SensorDataSender(url);

            _distanceSensor = new DistanceSensorHCSR04();
            _distanceSensor.InitGPIO();

            InitTimer(3000);
        }

        private void InitTimer(int readRate)
        {
            timer = new Timer(ReadDistance, null, 0, readRate);
        }

        private void ReadDistance(object state)
        {
            var distanceReading = _distanceSensor.Read();
            if (distanceReading == null)
            {
                return;
            }
            sensorDataSender.SendObjectAsJson(new SensorReading
            {
                name = "Distance sensor 1",
                id = "room1_dist",
                data = new List<Measurement>
                {
                    new Measurement
                    {
                        measurement_time = DateTime.Now,
                        value = distanceReading.DistanceInCm.ToString(),
                        unit = "cm",
                    }
                }
            });
        }
    }
}
