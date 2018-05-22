using System;

namespace DemoCluster.GrainInterfaces.States
{
    [Serializable]
    public class DeviceInstanceState
    {
        public Guid DeviceId { get; set; }
        public string Name { get; set; }
    }
}