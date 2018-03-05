using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoCluster.DAL.Database;
using DemoCluster.DAL.ViewModels;
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
    }
}