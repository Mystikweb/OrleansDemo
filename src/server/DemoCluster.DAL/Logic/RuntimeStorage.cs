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
        private readonly IMongoCollection<DeviceInstanceHistory> deviceHistoryCollection;
        private readonly IMongoCollection<SensorSummary> sensorSummaryCollection;

        public RuntimeStorage(IMongoDatabase db, MongoDbOptions mongoOptions)
        {
            deviceHistoryCollection = db.GetCollection<DeviceInstanceHistory>(mongoOptions.DeviceHistoryCollection);
            sensorSummaryCollection = db.GetCollection<SensorSummary>(mongoOptions.SensorStateCollection);
        }

        public async Task<List<DeviceInstanceHistory>> GetDeviceStateHistory(Guid deviceId, int days = 30)
        {
            DateTime startDateUtc = DateTime.UtcNow.AddDays((-1 * days));
            FilterDefinitionBuilder<DeviceInstanceHistory> builder = Builders<DeviceInstanceHistory>.Filter;
            FilterDefinition<DeviceInstanceHistory> filter = builder.Eq(d => d.DeviceId, deviceId.ToString()) 
                & builder.Gte(d => d.Timestamp, startDateUtc);

            return await deviceHistoryCollection.Find(filter).ToListAsync();
        }

        public async Task<bool> SaveDeviceState(DeviceInstanceHistory item)
        {
            bool result = true;

            try
            {
                await deviceHistoryCollection.InsertOneAsync(item);
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}