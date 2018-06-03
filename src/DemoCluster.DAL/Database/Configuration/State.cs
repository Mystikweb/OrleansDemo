using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoCluster.DAL.Database.Configuration
{
    public partial class State
    {
        public State()
        {
            DeviceState = new HashSet<DeviceState>();
        }

        public int StateId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [InverseProperty("State")]
        public ICollection<DeviceState> DeviceState { get; set; }
    }
}
