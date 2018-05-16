using System;
using System.Collections.Generic;

namespace DemoCluster.DAL.Models
{
    public class DeviceHistoryItem
    {
        public string DeviceId { get; set; }
        public string Name { get; set; }
        public bool IsRunning { get; set; } = false;
        public long Timestamp { get; set; }
        public List<SensorStatusItem> Sensors { get; set; } = new List<SensorStatusItem>();
    }
}