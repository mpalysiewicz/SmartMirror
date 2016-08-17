using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABB.Sensors.Motion
{
    public static class MotionSensorFactory
    {
        public static IMotionSensor Create()
        {
            return new MotionSensorHCSR501();
        }
    }
}
