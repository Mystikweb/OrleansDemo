using System;
using System.Collections.Generic;

namespace DemoCluster.DAL.Models
{
    public class DeviceSummary
    {
        public Guid DeviceId { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public List<SensorSummary> SensorSummaries { get; set; } = new List<SensorSummary>();
    }

    public class SensorSummary
    {
        public string Name { get; set; }
        public double Average { get; set; }
        public string Uom { get; set; }
    }

    public class DeviceStateItem
    {
        public Guid DeviceId { get; set; }
        public string Name { get; set; }
        public DateTime TimeStamp { get; set; }
        public int SensorCount { get; set; }
        public int EventTypeCount { get; set; }
    }
}