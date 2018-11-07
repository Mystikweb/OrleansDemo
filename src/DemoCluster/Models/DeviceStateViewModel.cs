namespace DemoCluster.Models
{
    public class DeviceStateViewModel
    {
        public int? DeviceStateId { get; set; }
        public string DeviceId { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public bool IsEnabled { get; set; }
    }
}
