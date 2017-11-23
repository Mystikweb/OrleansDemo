using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrleansDemo.Models.Configuration
{
    [Table("Device", Schema = "Configuration")]
    public partial class Device
    {
        public Device()
        {
            Readings = new HashSet<Reading>();
        }

        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public int DeviceTypeId { get; set; }
        public bool? Enabled { get; set; }
        public bool? RunOnStartup { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(75)]
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [StringLength(75)]
        public string UpdatedBy { get; set; }

        [ForeignKey("DeviceTypeId")]
        [InverseProperty("Devices")]
        public DeviceType DeviceType { get; set; }
        [InverseProperty("Device")]
        public ICollection<Reading> Readings { get; set; }
    }
}
