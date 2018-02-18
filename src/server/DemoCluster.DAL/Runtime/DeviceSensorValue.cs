using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoCluster.DAL.Runtime
{
    public partial class DeviceSensorValue
    {
        [StringLength(100)]
        public string Device { get; set; }
        [StringLength(100)]
        public string Sensor { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime Timestamp { get; set; }
        [Required]
        [StringLength(100)]
        public string Value { get; set; }
    }
}
