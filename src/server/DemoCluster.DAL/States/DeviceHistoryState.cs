using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCluster.DAL.States
{
    [Serializable]
    public class DeviceHistoryState
    {
        public Guid DeviceId { get; set; }
        public bool IsRunning { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public int SensorCount { get; set; } = 0;
        public int EventTypeCount { get; set; } = 0;
    }
}
