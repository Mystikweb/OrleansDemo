using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoCluster.DAL.Database
{
    [Table("DeviceSensorValue", Schema = "Runtime")]
    public partial class DeviceSensorValue
    {
        public Guid DeviceId { get; set; }
        public int SensorId { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime Timestamp { get; set; }
        [Required]
        [StringLength(100)]
        public string Value { get; set; }

        [ForeignKey("DeviceId")]
        [InverseProperty("DeviceSensorValue")]
        public Device Device { get; set; }
        [ForeignKey("SensorId")]
        [InverseProperty("DeviceSensorValue")]
        public Sensor Sensor { get; set; }
    }
}
