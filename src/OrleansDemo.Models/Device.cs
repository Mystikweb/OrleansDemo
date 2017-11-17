using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrleansDemo.Models
{
    [Table("Device", Schema = "Runtime")]
    public partial class Device
    {
        public Device()
        {
            Configurations = new HashSet<Configuration>();
        }

        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [InverseProperty("Device")]
        public ICollection<Configuration> Configurations { get; set; }
    }
}
