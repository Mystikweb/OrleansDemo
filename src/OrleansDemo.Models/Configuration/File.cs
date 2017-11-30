using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrleansDemo.Models.Configuration
{
    [Table("File", Schema = "Configuration")]
    public partial class File
    {
        public File()
        {
            DeviceTypes = new HashSet<DeviceType>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        [Required]
        [StringLength(10)]
        public string Extension { get; set; }
        [Required]
        [StringLength(50)]
        public string MimeType { get; set; }
        public int FileType { get; set; }
        [Required]
        public byte[] Data { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        [StringLength(100)]
        public string UpdatedBy { get; set; }

        [InverseProperty("File")]
        public ICollection<DeviceType> DeviceTypes { get; set; }
    }
}
