using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DemoCluster.Models
{
    public class DeviceViewModel
    {
        public string DeviceId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public bool IsEnabled { get; set; } = false;
        public List<DeviceSensorViewModel> Sensors { get; set; } = new List<DeviceSensorViewModel>();
        public List<DeviceEventTypeViewModel> EventTypes { get; set; } = new List<DeviceEventTypeViewModel>();
        public List<DeviceStateViewModel> States { get; set; } = new List<DeviceStateViewModel>();
    }
}
