using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoCluster.DAL.Configuration
{
    public partial class DeviceEventType
    {
        public int DeviceEventTypeId { get; set; }
        public Guid DeviceId { get; set; }
        public int EventTypeId { get; set; }
        public bool IsEnabled { get; set; }

        [ForeignKey("DeviceId")]
        [InverseProperty("DeviceEventType")]
        public Device Device { get; set; }
        [ForeignKey("EventTypeId")]
        [InverseProperty("DeviceEventType")]
        public EventType EventType { get; set; }
    }
}
