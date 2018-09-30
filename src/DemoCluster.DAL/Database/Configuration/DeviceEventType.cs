using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoCluster.DAL.Database.Configuration
{
    public partial class DeviceEventType
    {
        private Action<object, string> Loader { get; set; }

        private Device _device;
        private EventType _eventType;

        public DeviceEventType() { }

        public DeviceEventType(Action<object, string> loader)
        {
            Loader = loader;
        }

        public int DeviceEventTypeId { get; set; }
        public Guid DeviceId { get; set; }
        public int EventTypeId { get; set; }
        public bool IsEnabled { get; set; }

        [ForeignKey("DeviceId")]
        [InverseProperty("DeviceEventType")]
        public Device Device 
        { 
            get => Loader.Load(this, ref _device);
            set => _device = value;
        }

        [ForeignKey("EventTypeId")]
        [InverseProperty("DeviceEventType")]
        public EventType EventType 
        { 
            get => Loader.Load(this, ref _eventType); 
            set => _eventType = value;
        }
    }
}
