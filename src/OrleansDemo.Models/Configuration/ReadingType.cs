using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrleansDemo.Models.Configuration
{
    [Table("ReadingType", Schema = "Configuration")]
    public partial class ReadingType
    {
        public ReadingType()
        {
            DeviceTypeReadingTypes = new HashSet<DeviceTypeReadingType>();
            Readings = new HashSet<Reading>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [Column("UOM")]
        [StringLength(10)]
        public string Uom { get; set; }
        [Required]
        [StringLength(50)]
        public string DataType { get; set; }
        [StringLength(50)]
        public string Assembly { get; set; }
        [StringLength(50)]
        public string Class { get; set; }
        [StringLength(50)]
        public string Method { get; set; }

        [InverseProperty("ReadingType")]
        public ICollection<DeviceTypeReadingType> DeviceTypeReadingTypes { get; set; }
        [InverseProperty("ReadingType")]
        public ICollection<Reading> Readings { get; set; }
    }
}
