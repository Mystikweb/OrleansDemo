using System;
using System.Collections.Generic;

namespace DemoCluster.DAL.Models
{
    public class DeviceStatusChange
    {
        public string DeviceId { get; set; }
        public bool IsRunning { get; set; }
    }

    public class DeviceStateItem
    {
        public Guid DeviceId { get; set; }
        public int DeviceStatusId { get; set; }
        public int StatusId { get; set; }
        public string Name { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}