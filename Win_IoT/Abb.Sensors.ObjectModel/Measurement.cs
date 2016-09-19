using System;

namespace Abb.Sensors.ObjectModel
{
    public class Measurement
    {
        public DateTime measurement_time { get; set; }
        public string value { get; set; }
        public string unit { get; set; }
}
}
