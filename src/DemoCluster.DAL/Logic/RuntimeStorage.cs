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
        private readonly IMongoCollection<DeviceStateHistory> deviceHistoryCollection;
        private readonly FilterDefinitionBuilder<DeviceStateHistory> filterBuilder = Builders<DeviceStateHistory>.Filter;
        private readonly SortDefinitionBuilder<DeviceStateHistory> sortBuilder = Builders<DeviceStateHistory>.Sort;

        public RuntimeStorage(IMongoDatabase db, RuntimeCollections collectionNames)
        {
            deviceHistoryCollection = db.GetCollection<DeviceStateHistory>(collectionNames.DeviceStatusHistory);
        }

        public async Task<List<DeviceStateHistory>> GetDeviceStateHistory(Guid deviceId, int days = 30)
        {
            DateTime startDateUtc = DateTime.UtcNow.AddDays((-1 * days));
            
            FilterDefinition<DeviceStateHistory> filter = filterBuilder.Eq(d => d.DeviceId, deviceId) 
                & filterBuilder.Gte(d => d.Timestamp, startDateUtc);
            SortDefinition<DeviceStateHistory> sort = sortBuilder.Descending(d => d.Version);

            return await deviceHistoryCollection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<bool> SaveDeviceState(DeviceStateHistory item)
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