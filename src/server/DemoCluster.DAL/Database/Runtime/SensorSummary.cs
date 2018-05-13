using System;
using MongoDB.Bson;

namespace DemoCluster.DAL.Database.Runtime
{
    public class SensorSummary
    {
        public ObjectId Id { get; set; }
        public int SensorId { get; set; }
        public string Name { get; set; }
        public Guid DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string UOM { get; set; }
        public bool IsReceiving { get; set; } = false;
        public long Timestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        public double Total { get; set; } = 0;
        public double Average { get; set; } = 0;
    }
}