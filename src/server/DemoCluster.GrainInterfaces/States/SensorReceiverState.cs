using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCluster.GrainInterfaces.States
{
    [Serializable]
    public class SensorReceiverState
    {
        public int DeviceSensorId { get; set; }
        public string Name { get; set; }
        public string Device { get; set; }
        public bool IsReceiving { get; set; } = false;
    }
}
