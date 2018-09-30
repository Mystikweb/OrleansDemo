using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoCluster.DAL.Database.Configuration
{
    public partial class DeviceSensor
    {
        private Action<object, string> Loader { get; set; }

        private Device _device;
        private Sensor _sensor;

        public DeviceSensor() { }

        public DeviceSensor(Action<object, string> loader)
        {
            Loader = loader;
        }

        public int DeviceSensorId { get; set; }
        public Guid DeviceId { get; set; }
        public int SensorId { get; set; }
        public bool IsEnabled { get; set; }

        [ForeignKey("DeviceId")]
        [InverseProperty("DeviceSensor")]
        public Device Device 
        { 
            get => Loader.Load(this, ref _device); 
            set => _device = value;
        }

        [ForeignKey("SensorId")]
        [InverseProperty("DeviceSensor")]
        public Sensor Sensor 
        { 
            get => Loader.Load(this, ref _sensor);
            set => _sensor = value;
        }
    }
}
