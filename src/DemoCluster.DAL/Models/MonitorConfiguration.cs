using System.ComponentModel.DataAnnotations;

namespace DemoCluster.DAL.Models
{
    public class MonitorConfig
    {
        public string MonitorId { get; set; }
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
        public bool IsEnabled { get; set; } = false;
        public bool RunAtStartup { get; set; } = false;
    }
}