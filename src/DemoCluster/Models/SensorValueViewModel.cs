using System;

namespace DemoCluster.Models
{
    public class SensorValueViewModel
    {
        public int DeviceSensorId { get; set; }
        public double Value { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
