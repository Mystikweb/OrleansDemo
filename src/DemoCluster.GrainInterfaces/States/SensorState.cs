using System;
using System.Collections.Generic;

namespace DemoCluster.GrainInterfaces.States
{
    [Serializable]
    public class SensorState
    {
        public int DeviceSensorId { get; set; }
        public int SensorId { get; set; }
        public string Name { get; set; }
        public string UOM { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime Timestamp { get; set; }
        public List<SensorStateValue> Values { get; set; } = new List<SensorStateValue>();
    }

    [Serializable]
    public class SensorStateValue
    {
        public double Value { get; set; }
        public DateTime Timestamp { get; set; }
    }
}