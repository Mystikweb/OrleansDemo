using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoCluster.DAL.Database.Configuration
{
    public partial class EventType
    {
        private Action<object, string> Loader { get; set; }

        private ICollection<DeviceEventType> _deviceEventType;

        public EventType() { }

        public EventType(Action<object, string> loader)
        {
            Loader = loader;
        }

        public int EventTypeId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [InverseProperty("EventType")]
        public ICollection<DeviceEventType> DeviceEventType 
        { 
            get => Loader.Load(this, ref _deviceEventType); 
            set => _deviceEventType = value;
        }
    }
}
