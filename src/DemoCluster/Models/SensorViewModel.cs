using System.ComponentModel.DataAnnotations;

namespace DemoCluster.Models
{
    public class SensorViewModel
    {
        public int? SensorId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Uom { get; set; }
    }
}
