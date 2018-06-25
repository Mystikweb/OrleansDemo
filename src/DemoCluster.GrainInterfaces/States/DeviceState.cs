using System;
using System.Collections.Generic;
using System.Text;
using DemoCluster.GrainInterfaces.Commands;

namespace DemoCluster.GrainInterfaces.States
{
    [Serializable]
    public class DeviceState
    {
        public Guid DeviceId { get; set; }
        public string Name { get; set; }
        public CurrentDeviceState CurrentState { get; set; }
    }

    [Serializable]
    public class CurrentDeviceState
    {
        public int DeviceStateId { get; set; }
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
    }

    [Serializable]
    public class DeviceSensorState
    {
        public int DeviceSensorId { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
