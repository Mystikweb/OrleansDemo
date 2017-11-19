using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrleansDemo.API.ViewModels
{
    public class DeviceViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DeviceType { get; set; }
        public bool Enabled { get; set; }
        public bool RunOnStartup { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
