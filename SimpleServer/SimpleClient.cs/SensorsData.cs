using System;
using System.Collections.Generic;

namespace SimpleClient.cs
{
    public class SensorsData
    {
        public string name { get; set; }
        public List<Measurement> data { get; set; }
    }

    public class Measurement
    {
        public DateTime measurement_time { get; set; }
        public string value { get; set; }
        public string unit { get; set; }
    }
}
