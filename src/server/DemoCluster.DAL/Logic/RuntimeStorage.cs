using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoCluster.DAL.Database;
using DemoCluster.DAL.Database.Runtime;
using DemoCluster.DAL.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DemoCluster.DAL
{
    public class RuntimeStorage : IRuntimeStorage
    {
        // private readonly IMongoCollection<DeviceHistory> deviceHistoryCollection;
        private readonly IMongoCollection<SensorSummary> sensorSummaryCollection;

        public RuntimeStorage(IMongoDatabase db, MongoDbOptions mongoOptions)
        {
            // deviceHistoryCollection = db.GetCollection<DeviceHistory>(mongoOptions.DeviceHistoryCollection);
            sensorSummaryCollection = db.GetCollection<SensorSummary>(mongoOptions.SensorStateCollection);
        }

        // public async Task<List<DeviceHistoryItem>> GetDeviceStateHistory(Guid deviceId, int days = 14)
        // {
        //     long earliest = DateTimeOffset.UtcNow.AddDays((days * -1)).ToUnixTimeSeconds();
        //     FilterDefinitionBuilder<DeviceHistory> builder = Builders<DeviceHistory>.Filter;
        //     FilterDefinition<DeviceHistory> filter = builder.Eq(d => d.DeviceId, deviceId.ToString()) 
        //         & builder.Gte(d => d.Timestamp, earliest);

        //     List<DeviceHistory> findResults = await deviceHistoryCollection.Find(filter).ToListAsync();

        //     return findResults.Select(d => d.ToDeviceHistoryItem()).ToList();
        // }

        // public async Task<DeviceHistoryItem> SaveDeviceState(DeviceHistoryItem item)
        // {
        //     await deviceHistoryCollection.InsertOneAsync(item.ToDeviceHistory());

        //     return item;
        // }
    }
}