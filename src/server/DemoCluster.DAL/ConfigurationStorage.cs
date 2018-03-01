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
                    DeviceId = d.DeviceId,
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

        public async Task<Guid> SaveDeviceAsync(DeviceConfig device)
        {
            Device dbDevice = null;

            if (device.DeviceId == null || device.DeviceId == Guid.Empty)
            {
                dbDevice = new Device();
                dbDevice.Name = device.Name;
                
                await configDb.Device.AddAsync(dbDevice);
            }
            else
            {
                dbDevice = await configDb.Device.FirstOrDefaultAsync(d => d.DeviceId == device.DeviceId);

                if (dbDevice == null)
                {
                    throw new ApplicationException($"Device with id {device.DeviceId.ToString()} was not found.");
                }

                dbDevice.Name = device.Name;

                configDb.Device.Update(dbDevice);
            }

            await configDb.SaveChangesAsync();

            return dbDevice.DeviceId;
        }
    }
}