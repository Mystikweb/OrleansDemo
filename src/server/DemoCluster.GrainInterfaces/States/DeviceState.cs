using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCluster.GrainInterfaces.States
{
    [Serializable]
    public class DeviceState
    {
        public Guid DeviceId { get; set; }
        public string Name { get; set; }
        public bool IsRunning { get; set; }
        public DateTime Timestamp { get; set; }
        
        public void Apply(DeviceUpdateEvent updateEvent)
        {
            DeviceId = updateEvent.DeviceId;
            Name = updateEvent.Name;
            IsRunning = updateEvent.IsRunning;
            Timestamp = updateEvent.Timestamp;
        }
    }
}
