﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoCluster.DAL.Database
{
    public partial class Device
    {
        public Device()
        {
            DeviceEventType = new HashSet<DeviceEventType>();
            DeviceHistory = new HashSet<DeviceHistory>();
            DeviceSensor = new HashSet<DeviceSensor>();
        }

        public Guid DeviceId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public bool IsEnabled { get; set; }

        [InverseProperty("Device")]
        public ICollection<DeviceEventType> DeviceEventType { get; set; }
        [InverseProperty("Device")]
        public ICollection<DeviceHistory> DeviceHistory { get; set; }
        [InverseProperty("Device")]
        public ICollection<DeviceSensor> DeviceSensor { get; set; }
    }
}
