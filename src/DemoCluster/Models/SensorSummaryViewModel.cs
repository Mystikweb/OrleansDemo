using System;

namespace DemoCluster.Models
{
    public class SensorSummaryViewModel
    {
        public int DeviceSensorId { get; set; }
        public int SensorId { get; set; }
        public string SensorName { get; set; }
        public string UOM { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsRunning { get; set; }
        public double? LastValue { get; set; }
        public DateTime? LastValueReceived { get; set; }
        public double? AverageValue { get; set; }
        public double? TotalValue { get; set; }
    }
}
