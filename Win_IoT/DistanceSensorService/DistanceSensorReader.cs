using ABB.Sensors.Distance;
using SensorDataForwarder;
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

            InitTimer(100);
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
            sensorDataSender.SendObjectAsJson(distanceReading);
        }
    }
}
