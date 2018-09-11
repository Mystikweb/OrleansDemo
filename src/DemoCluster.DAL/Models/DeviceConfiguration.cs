using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DemoCluster.DAL.Models
{
    public class DeviceConfig
    {
        public string DeviceId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public bool IsEnabled { get; set; } = false;
        public IEnumerable<DeviceSensorConfig> Sensors { get; set; } = new List<DeviceSensorConfig>();
        public IEnumerable<DeviceEventConfig> Events { get; set; } = new List<DeviceEventConfig>();
        public IEnumerable<DeviceStateConfig> States { get; set; } = new List<DeviceStateConfig>();
    }

    public class DeviceSensorConfig
    {
        public int? DeviceSensorId { get; set; }
        public string DeviceId { get; set; }
        public int SensorId { get; set; }
        public string Name { get; set; }
        public string UOM { get; set; }
        public bool IsEnabled { get; set; } = false;
    }

    public class DeviceEventConfig
    {
        public int? DeviceEventTypeId { get; set; }
        public string DeviceId { get; set; }
        public int EventTypeId { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; } = false;
    }

    public class DeviceStateConfig
    {
        public int? DeviceStateId { get; set; }
        public string DeviceId { get; set; }
        public int StateId { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
    }
}