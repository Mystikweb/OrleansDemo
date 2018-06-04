using System;
using System.Collections.Generic;
using DemoCluster.GrainInterfaces.States;

namespace DemoCluster.GrainInterfaces.Commands
{
    public abstract class DeviceCommand
    {
        public DeviceCommand(DateTime? timeStamp, int? version)
        {
            this.Timestamp = timeStamp.HasValue ? timeStamp.Value : DateTime.UtcNow;
            this.Version = version.HasValue ? version.Value : 1; 
        }
        public Guid DeviceId { get; set; }
        public DateTime Timestamp { get; private set; }
        public int? Version { get; private set; }
    }
}