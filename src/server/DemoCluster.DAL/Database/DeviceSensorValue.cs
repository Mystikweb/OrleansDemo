using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoCluster.DAL.Database
{
    [Table("DeviceSensorValue", Schema = "Runtime")]
    public partial class DeviceSensorValue
    {
        public int DeviceSensorId { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }

        [ForeignKey("DeviceSensorId")]
        [InverseProperty("DeviceSensorValue")]
        public DeviceSensor DeviceSensor { get; set; }
    }
}
