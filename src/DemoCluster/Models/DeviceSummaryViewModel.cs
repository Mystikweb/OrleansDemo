using System;
using System.Collections.Generic;

namespace DemoCluster.Models
{
    public class DeviceSummaryViewModel
    {
        public string DeviceId { get; set; }
        public string Name { get; set; }
        public bool IsRunning { get; set; }
        public DateTime Timestamp { get; set; }
        public List<SensorSummaryViewModel> Sensors { get; set; } = new List<SensorSummaryViewModel>();
    }
}
