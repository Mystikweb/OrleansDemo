namespace DemoCluster.Models
{
    public class DeviceSensorViewModel
    {
        public int? DeviceSensorId { get; set; }
        public string DeviceId { get; set; }
        public int SensorId { get; set; }
        public string SensorName { get; set; }
        public string UOM { get; set; }
        public bool IsEnabled { get; set; } = false;
    }
}
