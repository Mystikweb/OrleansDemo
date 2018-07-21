using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace DemoCluster.DAL.Database.Runtime
{
    public class DeviceHistory
    {
        public ObjectId Id { get; set; }
        public Guid DeviceId { get; set; }
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
        public int Version { get; set; }
        public DeviceStateItem State { get; set; } = new DeviceStateItem();
        public List<SensorItem> Sensors { get; set; } = new List<SensorItem>();
    }

    public class DeviceStateItem
    {
        public int DeviceStateId { get; set; }
        public int StateId { get; set; }
        public string Name { get; set; }
    }

    public class SensorItem
    {
        public int DeviceSensorId { get; set; }
        public int SensorId { get; set; }
        public string Name { get; set; }
        public string UOM { get; set; }
        public bool Enabled { get; set; }
    }
}