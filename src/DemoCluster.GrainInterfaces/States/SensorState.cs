namespace DemoCluster.GrainInterfaces.States
{
    public class SensorState
    {
        public int DeviceSensorId { get; set; }
        public string Name { get; set; }
        public bool IsReceiving { get; set; }
        public double? CurrentValue { get; set; }
        public double? AverageValue { get; set; }
    }
}