using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoCluster.DAL.Database.Configuration
{
    public partial class State
    {
        private Action<object, string> Loader { get; set; }

        private ICollection<DeviceState> _deviceState;

        public State() { }

        public State(Action<object, string> loader)
        {
            Loader = loader;
        }

        public int StateId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [InverseProperty("State")]
        public ICollection<DeviceState> DeviceState 
        { 
            get => Loader.Load(this, ref _deviceState);
            set => _deviceState = value;
        }
    }
}
