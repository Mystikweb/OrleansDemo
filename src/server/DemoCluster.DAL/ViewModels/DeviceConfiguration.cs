using System;
using System.Collections.Generic;

namespace DemoCluster.DAL.ViewModels
{
    public class DeviceConfig
    {
        public string DeviceId { get; set; }
        public string Name { get; set; }
        public List<DeviceSensorConfig> Sensors { get; set; } = new List<DeviceSensorConfig>();
        public List<DeviceEventConfig> Events { get; set; } = new List<DeviceEventConfig>();
    }

    public class DeviceSensorConfig
    {
        public int? DeviceSensorId { get; set; }
        public int SensorId { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; } = false;
    }

    public class DeviceEventConfig
    {
        public int? DeviceEventTypeId { get; set; }
        public int EventTypeId { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; } = false;
    }
}