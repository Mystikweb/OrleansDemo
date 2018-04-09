using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoCluster.DAL.Database
{
    public partial class Sensor
    {
        public Sensor()
        {
            DeviceSensor = new HashSet<DeviceSensor>();
        }

        public int SensorId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [Column("UOM")]
        [StringLength(50)]
        public string Uom { get; set; }

        [InverseProperty("Sensor")]
        public ICollection<DeviceSensor> DeviceSensor { get; set; }
    }
}
