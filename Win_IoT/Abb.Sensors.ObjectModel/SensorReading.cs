using System.Collections.Generic;

namespace Abb.Sensors.ObjectModel
{
    public class SensorReading
    {
        public string name { get; set; }
        public List<Measurement> data { get; set; }

        public SensorReading()
        {
            data = new List<Measurement>();
        }
    }
}
