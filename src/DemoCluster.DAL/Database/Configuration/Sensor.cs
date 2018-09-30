using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoCluster.DAL.Database.Configuration
{
    public partial class Sensor
    {
        private Action<object, string> Loader { get; set; }

        private ICollection<DeviceSensor> _deviceSensor;

        public Sensor() { }

        public Sensor(Action<object, string> loader)
        {
            Loader = loader;
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
        public ICollection<DeviceSensor> DeviceSensor 
        { 
            get => Loader.Load(this, ref _deviceSensor);
            set => _deviceSensor = value;
        }
    }
}
