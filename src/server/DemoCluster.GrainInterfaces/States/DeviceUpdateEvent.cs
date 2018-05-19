using System;
using System.Collections.Generic;

namespace DemoCluster.GrainInterfaces.States
{
    public class DeviceUpdateEvent
    {
        public Guid DeviceId { get; set; }
        public string Name { get; set; }
        public bool IsRunning { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public List<SensorState> Sensors { get; set; } = new List<SensorState>();
    }
}