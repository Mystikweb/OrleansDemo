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

        public async Task<List<DeviceSummary>> GetDashboardSummary()
        {
            return await (from sv in db.DeviceSensorValue
                          where sv.Device.IsEnabled
                          group sv by sv.Device into dg
                          select new DeviceSummary
                          {
                              DeviceId = dg.Key.DeviceId.ToString(),
                              Name = dg.Key.Name,
                              SensorSummaries = (from s in dg
                                                 group s by s.Sensor into vs
                                                 select new SensorSummary
                                                 {
                                                     Name = vs.Key.Name,
                                                     Uom = vs.Key.Uom,
                                                     Average = vs.Sum(v => v.Value) / vs.Count()
                                                 }).ToList()

                          }).ToListAsync();
        }

        public async Task<List<DeviceStateItem>> GetDeviceStates()
        {
            return await db.Device.Select(d => new DeviceStateItem
            {
                DeviceId = d.DeviceId.ToString(),
                Name = d.Name
            }).ToListAsync();
        }

        public async Task<DeviceStateItem> GetInitialState(Guid deviceId)
        {
            return await db.Device.Select(d => new DeviceStateItem
            {
                DeviceId = d.DeviceId.ToString(),
                Name = d.Name
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