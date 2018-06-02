
using System;
using System.Collections.Generic;

namespace DemoDevice
{
    public class DeviceConfig
    {
        public Guid DeviceId { get; set; }
        public string Name { get; set; }
        public List<SensorConfig> Sensors { get; set; } = new List<SensorConfig>();
    }

    public class SensorConfig
    {
        public int DeviceSensorId { get; set; }
        public string Name { get; set; }        
    }
}