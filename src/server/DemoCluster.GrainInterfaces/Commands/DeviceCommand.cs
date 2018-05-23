using System;

namespace DemoCluster.GrainInterfaces.Commands
{
    public abstract class DeviceCommand
    {
        public Guid DeviceId { get; set; }
    }
}