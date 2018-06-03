using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoCluster.DAL.Database.Configuration
{
    public partial class DeviceState
    {
        public int DeviceStateId { get; set; }
        public Guid DeviceId { get; set; }
        public int StateId { get; set; }
        public bool IsEnabled { get; set; }

        [ForeignKey("DeviceId")]
        [InverseProperty("DeviceState")]
        public Device Device { get; set; }
        [ForeignKey("StateId")]
        [InverseProperty("DeviceState")]
        public State State { get; set; }
    }
}
