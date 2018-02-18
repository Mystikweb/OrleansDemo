using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoCluster.DAL.Configuration
{
    public partial class EventType
    {
        public EventType()
        {
            DeviceEventType = new HashSet<DeviceEventType>();
        }

        public int EventTypeId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [InverseProperty("EventType")]
        public ICollection<DeviceEventType> DeviceEventType { get; set; }
    }
}
