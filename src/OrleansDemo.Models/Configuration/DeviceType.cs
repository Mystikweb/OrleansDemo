using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrleansDemo.Models.Configuration
{
    [Table("DeviceType", Schema = "Configuration")]
    public partial class DeviceType
    {
        public DeviceType()
        {
            DeviceTypeReadingTypes = new HashSet<DeviceTypeReadingType>();
            Devices = new HashSet<Device>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public bool? Active { get; set; }
        public int? FileId { get; set; }

        [ForeignKey("FileId")]
        [InverseProperty("DeviceTypes")]
        public File File { get; set; }
        [InverseProperty("DeviceType")]
        public ICollection<DeviceTypeReadingType> DeviceTypeReadingTypes { get; set; }
        [InverseProperty("DeviceType")]
        public ICollection<Device> Devices { get; set; }
    }
}
