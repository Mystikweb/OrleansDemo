using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCluster.DAL.Models
{
    public class SensorStatusItem
    {
        public int DeviceSensorId { get; set; }
        public string Name { get; set; }
        public string UOM { get; set; }
        public bool IsReceiving { get; set; }
    }

    public class SensorValueItem
    {
        public int DeviceSensorId { get; set; }
        public DateTime TimeStamp { get; set; }
        public double Value { get; set; }
    }
}
