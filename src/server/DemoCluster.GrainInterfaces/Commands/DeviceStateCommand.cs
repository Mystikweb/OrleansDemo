using System;
using System.Collections.Generic;
using DemoCluster.GrainInterfaces.States;

namespace DemoCluster.GrainInterfaces.Commands
{
    public class DeviceStateCommand : DeviceCommand
    {
        public string Name { get; set; }
        public DeviceStatusState CurrentStatus { get; set; }
        public List<DeviceSensorState> Sensors { get; set; } = new List<DeviceSensorState>();
    }
}