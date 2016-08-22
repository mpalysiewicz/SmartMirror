using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABB.Sensors.Distance
{
    public static class DistanceSensorFactory
    {
        public static IDistanceSensor Create()
        {
            return new DistanceSensorHCSR04();
        }
    }
}
