using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCluster.GrainInterfaces.States
{
    public class SensorValueState
    {
        public int DeviceSensorId { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
