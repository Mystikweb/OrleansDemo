using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace DemoCluster.DAL.Database.Runtime
{
    public class DeviceInstanceHistory
    {
        public ObjectId Id { get; set; }
        public string DeviceId { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
        public DeviceStatus Status { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public List<SensorStatus> Sensors { get; set; } = new List<SensorStatus>();
    }

    public class DeviceStatus
    {
        public int DeviceStateId { get; set; }
        public string Name { get; set; }
    }

    public class SensorStatus
    {
        public int DeviceSensorId { get; set; }
        public string Name { get; set; }
        public string UOM { get; set; }
        public bool IsReceiving { get; set; }
    }
}