using System;
using MongoDB.Bson;

namespace DemoCluster.DAL.Database.Runtime
{
    public class DeviceStateHistory
    {
        public ObjectId Id { get; set; }
        public Guid DeviceId { get; set; }
        public string Name { get; set; }
        public int DeviceStateId { get; set; }
        public string StateName { get; set; }
        public DateTime Timestamp { get; set; }
        public int Version { get; set; }
    }
}