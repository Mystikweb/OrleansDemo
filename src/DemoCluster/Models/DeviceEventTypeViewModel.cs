namespace DemoCluster.Models
{
    public class DeviceEventTypeViewModel
    {
        public int? DeviceEventTypeId { get; set; }
        public string DeviceId { get; set; }
        public int EventTypeId { get; set; }
        public string EventTypeName { get; set; }
        public bool IsEnabled { get; set; } = false;
    }
}
