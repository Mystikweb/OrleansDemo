using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoCluster.DAL.Database.Configuration
{
    [Table("Monitor", Schema = "App")]
    public partial class Monitor
    {
        public Guid MonitorId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        public string HostName { get; set; }
        [StringLength(100)]
        public string UserName { get; set; }
        public string Password { get; set; }
        [Required]
        [StringLength(100)]
        public string ExchangeName { get; set; }
        [Required]
        [StringLength(100)]
        public string QueueName { get; set; }
        public bool IsEnabled { get; set; }
        public bool RunAtStartup { get; set; }
    }
}
