using System;
using System.Collections.Generic;

namespace DemoCluster.DAL.ViewModels
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
}