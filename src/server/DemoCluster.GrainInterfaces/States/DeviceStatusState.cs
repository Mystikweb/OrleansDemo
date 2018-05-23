using System;

namespace DemoCluster.GrainInterfaces.States
{
    [Serializable]
    public class DeviceStatusState
    {
        public int DeviceStateId { get; set; }
        public string Name { get; set; }
    }
}