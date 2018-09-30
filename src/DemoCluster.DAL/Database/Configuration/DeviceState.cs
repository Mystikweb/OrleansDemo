using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoCluster.DAL.Database.Configuration
{
    public partial class DeviceState
    {
        private Action<object, string> Loader { get; set; }

        private Device _device;
        private State _state;

        public DeviceState() { }

        public DeviceState(Action<object, string> loader)
        {
            Loader = loader;
        }

        public int DeviceStateId { get; set; }
        public Guid DeviceId { get; set; }
        public int StateId { get; set; }
        public bool IsEnabled { get; set; }

        [ForeignKey("DeviceId")]
        [InverseProperty("DeviceState")]
        public Device Device 
        { 
            get => Loader.Load(this, ref _device);
            set => _device = value;
        }

        [ForeignKey("StateId")]
        [InverseProperty("DeviceState")]
        public State State 
        { 
            get => Loader.Load(this, ref _state);
            set => _state = value;
        }
    }
}
