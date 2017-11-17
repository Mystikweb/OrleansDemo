using System;
using System.Collections.Generic;

namespace OrleansDemo.Models.Transfer
{
    public class DeviceConfiguration
    {
        public Guid DeviceId { get; set; }
        public string Name { get; set; }
        public string DeviceType { get; set; }
        public bool IsEnabled { get; set; }
        public IEnumerable<DeviceReadingConfiguration> ReadingConfigurations { get; set; } = new List<DeviceReadingConfiguration>();
    }

    public class DeviceReadingConfiguration
    {
        public string ReadingType { get; set; }
        public string UOM { get; set; }
        public string DataType { get; set; }
    }
}
