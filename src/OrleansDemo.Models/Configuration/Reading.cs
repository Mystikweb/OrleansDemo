using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrleansDemo.Models.Configuration
{
    [Table("Reading", Schema = "Configuration")]
    public partial class Reading
    {
        public Guid Id { get; set; }
        public Guid DeviceId { get; set; }
        public int ReadingTypeId { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(75)]
        public string UpdatedBy { get; set; }

        [ForeignKey("DeviceId")]
        [InverseProperty("Readings")]
        public Device Device { get; set; }
        [ForeignKey("ReadingTypeId")]
        [InverseProperty("Readings")]
        public ReadingType ReadingType { get; set; }
    }
}
