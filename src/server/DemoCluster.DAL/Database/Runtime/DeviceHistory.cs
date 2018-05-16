using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace DemoCluster.DAL.Database.Runtime
{
    public class DeviceHistory
    {
        public ObjectId Id { get; set; }
        public string DeviceId { get; set; }
        public string Name { get; set; }
        public bool IsRunning { get; set; } = false;
        public long Timestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        public List<SensorStatus> Sensors { get; set; } = new List<SensorStatus>();
    }

    public class SensorStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UOM { get; set; }
        public bool IsReceiving { get; set; }
    }
}