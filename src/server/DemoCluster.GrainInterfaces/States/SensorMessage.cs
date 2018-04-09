using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCluster.GrainInterfaces.States
{
    [Serializable]
    public class SensorMessage
    {
        public int DeviceSensorId { get; set; }
        public double Value { get; set; }
    }
}
