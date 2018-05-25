namespace DemoCluster.GrainInterfaces.States
{
    public class DeviceSensorState
    {
        public int DeviceSensorId { get; set; }
        public string Name { get; set; }
        public string UOM { get; set; }
        public bool IsReceiving { get; set; }
    }
}