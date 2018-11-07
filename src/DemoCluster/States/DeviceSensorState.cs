using System;

namespace DemoCluster.States
{
    [Serializable]
    public class DeviceSensorState
    {
        public int DeviceSensorId { get; set; }
        public int SensorId { get; set; }
        public string Name { get; set; }
        public string UOM { get; set; }
        public bool Enabled { get; set; }
        public double? LastValue { get; set; }
        public DateTime? LastValueReceived { get; set; }
        public double? AverageValue { get; set; }
        public double? TotalValue { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
