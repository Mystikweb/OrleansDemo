using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCluster.GrainInterfaces.States
{
    [Serializable]
    public class DeviceHistoryState
    {
        public Guid DeviceId { get; set; }
        public string Name { get; set; }
        public bool IsRunning { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public int EventTypeCount { get; set; } = 0;
        public List<SensorStatusState> SensorStatus { get; set; } = new List<SensorStatusState>();
    }

    [Serializable]
    public class SensorStatusState
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UOM { get; set; }
        public bool IsReceiving { get; set; }
    }
}
