using System;
using System.Collections.Generic;
using DemoCluster.GrainInterfaces.States;

namespace DemoCluster.GrainInterfaces.Commands
{
    public abstract class DeviceCommand
    {
        public DeviceCommand(DateTime? timeStamp)
        {
            this.Timestamp = timeStamp.HasValue ? timeStamp.Value : DateTime.UtcNow;
        }
        public Guid DeviceId { get; set; }
        public DateTime Timestamp { get; private set; }
    }
}