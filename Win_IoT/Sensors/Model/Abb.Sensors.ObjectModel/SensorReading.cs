using System.Collections.Generic;

namespace ABB.Sensors.ObjectModel
{
    public class SensorReading
    {
        public string name { get; set; }
        public string id { get; set; }
        public List<Measurement> data { get; set; }

        public SensorReading()
        {
            data = new List<Measurement>();
        }
    }
}
