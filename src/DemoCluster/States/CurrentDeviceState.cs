using System;

namespace DemoCluster.States
{
    [Serializable]
    public class CurrentDeviceState
    {
        public int DeviceStateId { get; set; }
        public int StateId { get; set; }
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
