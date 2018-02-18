using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoCluster.DAL.Configuration
{
    public partial class DeviceSensor
    {
        public int DeviceSensorId { get; set; }
        public Guid DeviceId { get; set; }
        public int SensorId { get; set; }
        public bool IsEnabled { get; set; }

        [ForeignKey("DeviceId")]
        [InverseProperty("DeviceSensor")]
        public Device Device { get; set; }
        [ForeignKey("SensorId")]
        [InverseProperty("DeviceSensor")]
        public Sensor Sensor { get; set; }
    }
}
