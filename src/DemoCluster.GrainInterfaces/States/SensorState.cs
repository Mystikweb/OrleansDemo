namespace DemoCluster.GrainInterfaces.States
{
    public class SensorState
    {
        public int DeviceSensorId { get; set; }
        public string Name { get; set; }
        public bool IsReceiving { get; set; }
    }
}