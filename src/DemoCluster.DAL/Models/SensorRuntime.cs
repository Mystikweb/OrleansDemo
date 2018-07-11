using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCluster.DAL.Models
{
    public class SensorValueItem
    {
        public int DeviceSensorId { get; set; }
        public double Value { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public class SensorStateSummary
    {
        public int DeviceSensorId { get; set; }
        public int SensorId { get; set; }
        public string Name { get; set; }
        public string UOM { get; set; }
        public bool IsEnabled { get; set; }
        public double? LastValue { get; set; }
        public DateTime? LastValueReceived { get; set; }
        public double? AverageValue { get; set; }
        public double? TotalValue { get; set; }
    }
}
