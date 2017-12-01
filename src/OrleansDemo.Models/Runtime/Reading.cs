using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrleansDemo.Models.Runtime
{
    [Table("Reading", Schema = "Runtime")]
    public partial class Reading
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Device { get; set; }
        [Required]
        [StringLength(50)]
        public string Type { get; set; }
        [Required]
        [Column("UOM")]
        [StringLength(10)]
        public string Uom { get; set; }
        [Required]
        [StringLength(50)]
        public string DataType { get; set; }
        [Required]
        [Column(TypeName = "nchar(10)")]
        public string Value { get; set; }
        public DateTime InsertedAt { get; set; }
    }
}
