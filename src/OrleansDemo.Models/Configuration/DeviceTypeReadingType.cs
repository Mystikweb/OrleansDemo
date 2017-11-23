using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrleansDemo.Models.Configuration
{
    [Table("DeviceTypeReadingType", Schema = "Configuration")]
    public partial class DeviceTypeReadingType
    {
        public int Id { get; set; }
        public int DeviceTypeId { get; set; }
        public int ReadingTypeId { get; set; }
        public bool? Active { get; set; }

        [ForeignKey("DeviceTypeId")]
        [InverseProperty("DeviceTypeReadingTypes")]
        public DeviceType DeviceType { get; set; }
        [ForeignKey("ReadingTypeId")]
        [InverseProperty("DeviceTypeReadingTypes")]
        public ReadingType ReadingType { get; set; }
    }
}
