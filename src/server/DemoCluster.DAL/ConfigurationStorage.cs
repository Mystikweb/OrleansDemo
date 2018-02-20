using DemoCluster.DAL.Configuration;
using DemoCluster.DAL.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoCluster.DAL
{
    public class ConfigurationStorage : IConfigurationStorage
    {
        private readonly ConfigurationContext configDb;

        public ConfigurationStorage(ConfigurationContext context)
        {
            configDb = context;
        }

        public async Task<List<DeviceConfig>> GetDeviceListAsync()
        {
            return await configDb.Device
                .Select(d => new DeviceConfig
                {
                    DeviceId = d.DeviceId,
                    Name = d.Name,
                    // Sensors = configDb.Sensor.GroupJoin()
                }).ToListAsync();
        }
    }
}