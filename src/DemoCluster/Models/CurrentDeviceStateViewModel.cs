using System;

namespace DemoCluster.Models
{
    public class CurrentDeviceStateViewModel
    {
        public Guid DeviceId { get; set; }
        public int DeviceStateId { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
