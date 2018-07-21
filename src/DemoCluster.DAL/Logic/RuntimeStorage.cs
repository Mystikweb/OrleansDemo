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
        private readonly IMongoCollection<DeviceHistory> deviceHistoryCollection;
        private readonly FilterDefinitionBuilder<DeviceHistory> deviceFilterBuilder = Builders<DeviceHistory>.Filter;
        private readonly SortDefinitionBuilder<DeviceHistory> deviceSortBuilder = Builders<DeviceHistory>.Sort;

        private readonly IMongoCollection<SensorStateHistory> sensorHistoryCollection;
        private readonly FilterDefinitionBuilder<SensorStateHistory> sensorFilterBuilder = Builders<SensorStateHistory>.Filter;
        private readonly SortDefinitionBuilder<SensorStateHistory> sensorSortBuilder = Builders<SensorStateHistory>.Sort;

        public RuntimeStorage(IMongoDatabase db, RuntimeCollections collectionNames)
        {
            deviceHistoryCollection = db.GetCollection<DeviceHistory>(collectionNames.DeviceStateHistory);
            sensorHistoryCollection = db.GetCollection<SensorStateHistory>(collectionNames.SensorStateHistory);
        }

        public async Task<List<DeviceHistory>> GetDeviceHistory(Guid deviceId, int days = 30)
        {
            DateTime startDateUtc = DateTime.UtcNow.AddDays((-1 * days));
            
            FilterDefinition<DeviceHistory> filter = deviceFilterBuilder.Eq(d => d.DeviceId, deviceId) 
                & deviceFilterBuilder.Gte(d => d.Timestamp, startDateUtc);
            SortDefinition<DeviceHistory> sort = deviceSortBuilder.Descending(d => d.Version);

            return await deviceHistoryCollection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<bool> SaveDeviceHistory(DeviceHistory item)
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

        // public async Task<List<SensorStateHistory>> GetSensorStateHistory(int deviceSensorId, int days = 30)
        // {
        //     DateTime startDateUtc = DateTime.UtcNow.AddDays((-1 * days));

        //     FilterDefinition<SensorStateHistory> filter = sensorFilterBuilder.Eq(s => s.DeviceSensorId, deviceSensorId)
        //         & sensorFilterBuilder.Gte(s => s.Timestamp, startDateUtc);
        //     SortDefinition<SensorStateHistory> sort = sensorSortBuilder.Descending(s => s.Version);

        //     return await sensorHistoryCollection.Find(filter).Sort(sort).ToListAsync();
        // }

        // public async Task<bool> SaveSensorState(SensorStateHistory item)
        // {
        //     bool result = true;

        //     try
        //     {
        //         await sensorHistoryCollection.InsertOneAsync(item);
        //     }
        //     catch
        //     {
        //         result = false;
        //     }

        //     return result;
        // }
    }
}