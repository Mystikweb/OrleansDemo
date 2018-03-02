using DemoCluster.DAL.Configuration;
using DemoCluster.DAL.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoCluster.DAL
{
    public class ConfigurationStorage : IConfigurationStorage
    {
        private readonly ConfigurationContext configDb;
        private readonly ILogger logger;

        public ConfigurationStorage(ConfigurationContext context, ILogger log)
        {
            configDb = context;
            logger = log;
        }

        public async Task<List<DeviceConfig>> GetDeviceListAsync()
        {
            return await configDb.Device
                .Select(d => new DeviceConfig
                {
                    DeviceId = d.DeviceId.ToString(),
                    Name = d.Name,
                    Sensors = configDb.Sensor.Select(s => new DeviceSensorConfig
                    {
                        DeviceSensorId = d.DeviceSensor.Where(ds => ds.SensorId == s.SensorId).Select(x => x.DeviceSensorId).FirstOrDefault(),
                        SensorId = s.SensorId,
                        Name = s.Name,
                        IsEnabled = d.DeviceSensor.Where(ds => ds.SensorId == s.SensorId).Select(x => x.IsEnabled).FirstOrDefault()
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<DeviceConfig> GetDeviceAsync(Guid deviceId)
        {
            return await configDb.Device
                .Where(d => d.DeviceId == deviceId)
                .Select(d => new DeviceConfig
                {
                    DeviceId = d.DeviceId.ToString(),
                    Name = d.Name,
                    Sensors = configDb.Sensor.Select(s => new DeviceSensorConfig
                    {
                        DeviceSensorId = d.DeviceSensor.Where(ds => ds.SensorId == s.SensorId).Select(x => x.DeviceSensorId).FirstOrDefault(),
                        SensorId = s.SensorId,
                        Name = s.Name,
                        IsEnabled = d.DeviceSensor.Where(ds => ds.SensorId == s.SensorId).Select(x => x.IsEnabled).FirstOrDefault()
                    }).ToList(),
                    Events = configDb.EventType.Select(e => new DeviceEventConfig
                    {
                        DeviceEventTypeId = d.DeviceEventType.Where(de => de.EventTypeId == e.EventTypeId).Select(x => x.DeviceEventTypeId).FirstOrDefault(),
                        EventTypeId = e.EventTypeId,
                        Name = e.Name,
                        IsEnabled = d.DeviceEventType.Where(de => de.EventTypeId == e.EventTypeId).Select(x => x.IsEnabled).FirstOrDefault()
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<DeviceConfig> SaveDeviceAsync(DeviceConfig device)
        {
            Device dbDevice = null;

            if (string.IsNullOrEmpty(device.DeviceId))
            {
                dbDevice = new Device();
                dbDevice.Name = device.Name;

                await configDb.Device.AddAsync(dbDevice);
            }
            else
            {
                dbDevice = await configDb.Device.FirstOrDefaultAsync(d => d.DeviceId == Guid.Parse(device.DeviceId));

                if (dbDevice == null)
                {
                    throw new ApplicationException($"Device with id {device.DeviceId.ToString()} was not found.");
                }

                dbDevice.Name = device.Name;

                configDb.Device.Update(dbDevice);
            }

            try
            {
                await configDb.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                logger.LogError(new EventId(5001), ex, string.Empty);
                throw;
            }

            return await GetDeviceAsync(dbDevice.DeviceId);
        }
    }
}