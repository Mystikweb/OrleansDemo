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
        public CurrentStatusState CurrentState { get; set; }
    }

    [Serializable]
    public class CurrentStatusState
    {
        public int DeviceStateId { get; set; }
        public string Name { get; set; }
    }
}
