using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrleansDemo.Models
{
    [Table("Configuration", Schema = "Runtime")]
    public partial class Configuration
    {
        public Guid Id { get; set; }
        public Guid DeviceId { get; set; }
        [Required]
        public string Data { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime UploadDate { get; set; }
        [Required]
        [StringLength(50)]
        public string UploadUser { get; set; }

        [ForeignKey("DeviceId")]
        [InverseProperty("Configurations")]
        public Device Device { get; set; }
    }
}
