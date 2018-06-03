using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoCluster.DAL.Database.Configuration
{
    public partial class Device
    {
        public Device()
        {
            DeviceEventType = new HashSet<DeviceEventType>();
            DeviceSensor = new HashSet<DeviceSensor>();
            DeviceState = new HashSet<DeviceState>();
        }

        public Guid DeviceId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public bool IsEnabled { get; set; }

        [InverseProperty("Device")]
        public ICollection<DeviceEventType> DeviceEventType { get; set; }
        [InverseProperty("Device")]
        public ICollection<DeviceSensor> DeviceSensor { get; set; }
        [InverseProperty("Device")]
        public ICollection<DeviceState> DeviceState { get; set; }
    }
}
