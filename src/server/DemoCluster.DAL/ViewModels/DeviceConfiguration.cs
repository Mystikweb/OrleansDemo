using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DemoCluster.DAL.ViewModels
{
    public class DeviceConfig
    {
        public string DeviceId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public IEnumerable<DeviceSensorConfig> Sensors { get; set; } = new List<DeviceSensorConfig>();
        public IEnumerable<DeviceEventConfig> Events { get; set; } = new List<DeviceEventConfig>();
    }

    public class DeviceSensorConfig
    {
        public int? DeviceSensorId { get; set; }
        public int SensorId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public bool IsEnabled { get; set; } = false;
    }

    public class DeviceEventConfig
    {
        public int? DeviceEventTypeId { get; set; }
        public int EventTypeId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public bool IsEnabled { get; set; } = false;
    }
}