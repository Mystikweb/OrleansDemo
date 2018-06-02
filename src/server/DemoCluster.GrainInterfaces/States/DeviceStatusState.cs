using System;
using DemoCluster.GrainInterfaces.Commands;

namespace DemoCluster.GrainInterfaces.States
{
    [Serializable]
    public class DeviceStatusState
    {
        public Guid DeviceId { get; set; }
        public int DeviceStateId { get; set; }
        public string Name { get; set; }

        public void Apply(DeviceStatusCommand statusUpdate)
        {
            this.DeviceStateId = statusUpdate.DeviceStateId;
            this.Name = statusUpdate.Name;
        }
    }
}