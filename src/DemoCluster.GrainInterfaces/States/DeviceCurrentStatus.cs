using System;

namespace DemoCluster.GrainInterfaces.States
{
    [Serializable]
    public class DeviceCurrentStatus
    {
        public int DeviceStateId { get; set; }
        public string Name { get; set; }
    }
}