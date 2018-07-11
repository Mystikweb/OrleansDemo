using System;
using System.Collections.Generic;
using DemoCluster.GrainInterfaces.States;

namespace DemoCluster.GrainInterfaces.Commands
{
    public abstract class DeviceCommand
    {
        public DeviceCommand(DateTime? timeStamp)
        {
            Timestamp = timeStamp.HasValue ? timeStamp.Value : DateTime.UtcNow;
        }
        public DateTime Timestamp { get; private set; }
    }
}