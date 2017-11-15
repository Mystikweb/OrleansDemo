using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrleansDemo.API.Models
{
    [Table("ReadingType", Schema = "Configuration")]
    public partial class ReadingType
    {
        public ReadingType()
        {
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

        [InverseProperty("ReadingType")]
        public ICollection<Reading> Readings { get; set; }
    }
}
