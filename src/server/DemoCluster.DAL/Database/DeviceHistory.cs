using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoCluster.DAL.Database
{
    [Table("DeviceHistory", Schema = "Runtime")]
    public partial class DeviceHistory
    {
        public Guid DeviceId { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime Timestamp { get; set; }
        public bool IsRunning { get; set; }
        public int SensorCount { get; set; }
        public int EventTypeCount { get; set; }

        [ForeignKey("DeviceId")]
        [InverseProperty("DeviceHistory")]
        public Device Device { get; set; }
    }
}
