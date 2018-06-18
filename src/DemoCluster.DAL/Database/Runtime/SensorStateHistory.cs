using System;
using MongoDB.Bson;

namespace DemoCluster.DAL.Database.Runtime
{
    public class SensorStateHistory
    {
        public ObjectId Id { get; set; }
        public int DeviceSensorId { get; set; }
        public string DeviceName { get; set; }
        public string SensorName { get; set; }
        public string Uom { get; set; }
        public bool IsReceiving { get; set; }
        public DateTime Timestamp { get; set; }
        public int Version { get; set; }
    }
}