using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoCluster.DAL.Runtime
{
    public partial class DeviceEvent
    {
        [StringLength(100)]
        public string Device { get; set; }
        [Required]
        [StringLength(100)]
        public string Event { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime StartTime { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? EndTime { get; set; }
    }
}
