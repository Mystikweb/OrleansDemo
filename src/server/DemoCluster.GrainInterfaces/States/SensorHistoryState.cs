using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCluster.GrainInterfaces.States
{
    public class SensorHistoryState
    {
        public int DeviceSensorId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public double TotalValue { get; set; } = 0;
        public double AverageValue { get; set; } = 0;
    }
}
