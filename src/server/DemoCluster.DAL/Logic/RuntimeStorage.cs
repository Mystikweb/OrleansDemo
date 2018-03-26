using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoCluster.DAL.Database;
using DemoCluster.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoCluster.DAL
{
    public class RuntimeStorage : IRuntimeStorage
    {
        private readonly RuntimeContext db;

        public RuntimeStorage(RuntimeContext context)
        {
            db = context;
        }

        public Task<List<DeviceSummary>> GetDashboardSummary()
        {
            return Task.FromResult(new List<DeviceSummary>());
        }

        public async Task<List<DeviceStateItem>> GetDeviceStates()
        {
            return await db.Device.Select(d => new DeviceStateItem
            {
                DeviceId = d.DeviceId.ToString(),
                Name = d.Name,
                Sensors = d.DeviceSensor.Where(ds => ds.IsEnabled).Select(s => new SensorStateItem
                {
                    DeviceSensorId = s.DeviceSensorId,
                    Name = s.Sensor.Name,
                    UOM = s.Sensor.Uom
                }).ToList()
            }).ToListAsync();
        }

        public async Task<DeviceStateItem> GetInitialState(Guid deviceId)
        {
            return await db.Device.Select(d => new DeviceStateItem
            {
                DeviceId = d.DeviceId.ToString(),
                Name = d.Name,
                Sensors = d.DeviceSensor.Where(ds => ds.IsEnabled).Select(s => new SensorStateItem
                {
                    DeviceSensorId = s.DeviceSensorId,
                    Name = s.Sensor.Name,
                    UOM = s.Sensor.Uom
                }).ToList()
            }).FirstOrDefaultAsync(x => x.DeviceId == deviceId.ToString());
        }

        public async Task<List<DeviceHistoryItem>> GetDeviceHistory(Guid deviceId)
        {
            return await db.DeviceHistory.Select(s => new DeviceHistoryItem
            {
                DeviceId = s.DeviceId,
                Name = s.Device.Name,
                IsRunning = s.IsRunning,
                TimeStamp = s.Timestamp,
                SensorCount = s.SensorCount,
                EventTypeCount = s.EventTypeCount
            }).ToListAsync();
        }

        public async Task<int> StoreDeviceHistory(DeviceHistoryItem historyItem)
        {
            DeviceHistory history = await db.DeviceHistory.FirstOrDefaultAsync(h => h.DeviceId == historyItem.DeviceId && h.Timestamp == historyItem.TimeStamp);
            if (history == null)
            {
                history = new DeviceHistory
                {
                    DeviceId = historyItem.DeviceId,
                    IsRunning = historyItem.IsRunning,
                    Timestamp = historyItem.TimeStamp,
                    SensorCount = historyItem.SensorCount,
                    EventTypeCount = historyItem.EventTypeCount
                };

                db.DeviceHistory.Add(history);

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return await db.DeviceHistory.Where(h => h.DeviceId == historyItem.DeviceId).CountAsync();
        }
    }
}