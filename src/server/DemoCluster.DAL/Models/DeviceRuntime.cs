using System;
using System.Collections.Generic;

namespace DemoCluster.DAL.Models
{
    public class DeviceInstanceItem
    {
        public string DeviceId { get; set; }
        public string Name { get; set; }
        public bool IsRunning { get; set; } = false;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public List<SensorStateItem> Sensors { get; set; } = new List<SensorStateItem>();
    }

    public class DeviceEventItem
    {
        public int DeviceEventId { get; set; }
        public string EventName { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class DeviceStatusChange
    {
        public string DeviceId { get; set; }
        public bool IsRunning { get; set; }
    }
}