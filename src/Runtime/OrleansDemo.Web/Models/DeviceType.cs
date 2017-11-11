using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrleansDemo.Web.Models
{
    [Table("DeviceType", Schema = "Configuration")]
    public partial class DeviceType
    {
        public DeviceType()
        {
            Devices = new HashSet<Device>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public bool? Active { get; set; }

        [InverseProperty("DeviceType")]
        public ICollection<Device> Devices { get; set; }
    }
}
