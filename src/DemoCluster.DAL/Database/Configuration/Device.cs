using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoCluster.DAL.Database.Configuration
{
    public partial class Device
    {
        private Action<object, string> Loader { get; set; }

        private ICollection<DeviceEventType> _deviceEventType;
        private ICollection<DeviceSensor> _deviceSensor;
        private ICollection<DeviceState> _deviceState;

        public Device() { }

        public Device(Action<object, string> loader)
        {
            Loader = loader;
        }

        public Guid DeviceId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public bool IsEnabled { get; set; }

        [InverseProperty("Device")]
        public ICollection<DeviceEventType> DeviceEventType 
        { 
            get => Loader.Load(this, ref _deviceEventType); 
            set => _deviceEventType = value;
        }

        [InverseProperty("Device")]
        public ICollection<DeviceSensor> DeviceSensor 
        { 
            get => Loader.Load(this, ref _deviceSensor); 
            set => _deviceSensor = value;
        }

        [InverseProperty("Device")]
        public ICollection<DeviceState> DeviceState 
        { 
            get => Loader.Load(this, ref _deviceState); 
            set => _deviceState = value;
        }
    }
}
