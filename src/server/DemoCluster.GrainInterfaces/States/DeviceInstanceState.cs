using System;
using DemoCluster.GrainInterfaces.Commands;

namespace DemoCluster.GrainInterfaces.States
{
    [Serializable]
    public class DeviceInstanceState
    {
        public Guid DeviceId { get; set; }
        public string Name { get; set; }
        public DeviceStatusState CurrentStatus { get; set; }

        public void Apply(DeviceStatusCommand statusUpdate)
        {
            this.CurrentStatus = statusUpdate.NewState;
        }
    }
}