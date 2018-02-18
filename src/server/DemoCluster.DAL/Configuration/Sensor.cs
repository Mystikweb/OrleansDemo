using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoCluster.DAL.Configuration
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

        [InverseProperty("Sensor")]
        public ICollection<DeviceSensor> DeviceSensor { get; set; }
    }
}
