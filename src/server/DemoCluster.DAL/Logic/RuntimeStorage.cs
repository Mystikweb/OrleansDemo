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
        private readonly IMongoCollection<DeviceState> deviceStateCollection;
        private readonly IMongoCollection<SensorSummary> sensorSummaryCollection;

        public RuntimeStorage(IMongoDatabase db, MongoDbOptions mongoOptions)
        {
            deviceStateCollection = db.GetCollection<DeviceState>(mongoOptions.DeviceStateCollection);
            sensorSummaryCollection = db.GetCollection<SensorSummary>(mongoOptions.SensorStateCollection);
        }

        public async Task<List<DeviceStateItem>> GetDeviceStateHistory(Guid deviceId, int days = 14)
        {
            long earliest = DateTimeOffset.UtcNow.AddDays((days * -1)).ToUnixTimeSeconds();
            FilterDefinition<DeviceState> filter = Builders<DeviceState>.Filter.Gte(d => d.Timestamp, earliest);

            List<DeviceState> findResults = await deviceStateCollection.Find(filter).ToListAsync();

            return findResults.Select(d => d.ToDeviceStateItem()).ToList();
        }

        public Task<DeviceStateItem> SaveDeviceState(DeviceStateItem item)
        {
            throw new NotImplementedException();
        }
    }
}