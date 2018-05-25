using System;
using System.Collections.Generic;
using DemoCluster.GrainInterfaces.States;

namespace DemoCluster.GrainInterfaces.Commands
{
    public abstract class DeviceCommand
    {
        public Guid DeviceId { get; set; }
    }
}