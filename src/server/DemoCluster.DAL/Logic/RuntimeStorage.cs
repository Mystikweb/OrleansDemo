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
                              DeviceId = dg.Key.DeviceId,
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

        public async Task<List<DeviceStateItem>> GetDeviceHistory(Guid deviceId)
        {
            return await db.DeviceHistory.Select(s => new DeviceStateItem
            {
                DeviceId = s.DeviceId,
                Name = s.Device.Name,
                TimeStamp = s.Timestamp,
                SensorCount = s.SensorCount,
                EventTypeCount = s.EventTypeCount
            }).ToListAsync();
        }

        public async Task<bool> StoreDeviceHistory(DeviceStateItem historyItem)
        {
            DeviceHistory history = await db.DeviceHistory.FirstOrDefaultAsync(h => h.DeviceId == historyItem.DeviceId && h.Timestamp == historyItem.TimeStamp);
            if (history == null)
            {
                history = new DeviceHistory
                {
                    DeviceId = historyItem.DeviceId,
                    Timestamp = historyItem.TimeStamp,
                    SensorCount = historyItem.SensorCount,
                    EventTypeCount = historyItem.EventTypeCount
                };

                db.DeviceHistory.Add(history);
            }
            else
            {
                history.SensorCount = historyItem.SensorCount;
                history.EventTypeCount = historyItem.EventTypeCount;

                db.DeviceHistory.Update(history);
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}