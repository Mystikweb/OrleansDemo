using System;

namespace DemoCluster.States
{
    [Serializable]
    public class SensorValueState
    {
        public double Value { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
