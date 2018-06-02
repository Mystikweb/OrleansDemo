using System;
using System.Collections.Generic;
using DemoCluster.GrainInterfaces.Commands;

namespace DemoCluster.GrainInterfaces.States
{
    [Serializable]
    public class DeviceStatusState
    {
        public Guid DeviceId { get; set; }
        public string DeviceName { get; set; }
        public int DeviceStateId { get; set; }
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
        public int Version { get; set; } = 0;
        public SortedDictionary<int, DeviceStatusHistory> History { get; set; } = new SortedDictionary<int, DeviceStatusHistory>();

        public void Apply(DeviceStatusCommand statusUpdate)
        {
            this.DeviceStateId = statusUpdate.DeviceStateId;
            this.Name = statusUpdate.Name;
            this.Timestamp = statusUpdate.Timestamp;
            this.Version = statusUpdate.Version.HasValue ? statusUpdate.Version.Value : this.Version++;
            
            if (this.History.ContainsKey(this.Version))
            {
                return;
            }

            this.History.Add(this.Version, new DeviceStatusHistory
            {
                DeviceStateId = this.DeviceStateId,
                Name = this.Name,
                Timestamp = this.Timestamp,
                Version = this.Version
            });
        }
    }

    public class DeviceStatusHistory
    {
        public int DeviceStateId { get; set; }
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
        public int Version { get; set; } = 0;
    }
}