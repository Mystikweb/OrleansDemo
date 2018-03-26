using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCluster.DAL.Models
{
    public class SensorSummary
    {
        public int DeviceSensorId { get; set; }
        public string Name { get; set; }
        public string Uom { get; set; }
    }

    public class SensorStateItem
    {
        public int DeviceSensorId { get; set; }
        public string Name { get; set; }
        public string UOM { get; set; }
    }

    public class SensoryValueItem
    {
        public int DeviceSensorId { get; set; }
        public DateTime TimeStamp { get; set; }
        public double Value { get; set; }
    }
}
