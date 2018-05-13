using System;
using System.Collections.Generic;

namespace DemoCluster.DAL.Models
{
    // public class DeviceSummary
    // {
    //     public string DeviceId { get; set; }
    //     public string Name { get; set; }
    //     public bool IsEnabled { get; set; }
    //     public List<SensorSummary> SensorSummaries { get; set; } = new List<SensorSummary>();
    // }

    public class DeviceStateItem
    {
        public string DeviceId { get; set; }
        public string Name { get; set; }
        public bool IsRunning { get; set; } = false;
        public long Timestamp { get; set; }
        public List<SensorStateItem> Sensors { get; set; } = new List<SensorStateItem>();
    }

    public class DeviceHistoryItem
    {
        public Guid DeviceId { get; set; }
        public string Name { get; set; }
        public bool IsRunning { get; set; }
        public DateTime Timestamp { get; set; }
        public int SensorCount { get; set; }
        public int EventTypeCount { get; set; }
    }
}