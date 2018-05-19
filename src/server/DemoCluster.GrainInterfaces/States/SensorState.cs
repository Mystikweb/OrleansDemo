using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCluster.GrainInterfaces.States
{
    [Serializable]
    public class SensorState
    {
        public int DeviceSensorId { get; set; }
        public string Name { get; set; }
        public string UOM { get; set; }
        public bool IsReceiving { get; set; }
    }
}
