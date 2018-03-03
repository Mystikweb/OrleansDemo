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
        public ConfigurationStorage(ConfigurationContext context)
        {
            configDb = context;
        }

        public async Task<List<DeviceConfig>> GetDeviceListAsync()
        {
            return await configDb.Device.Select(d => new DeviceConfig
            {
                DeviceId = d.DeviceId.ToString(),
                Name = d.Name,
                Sensors = d.DeviceSensor.Select(ds => new DeviceSensorConfig
                {
                    DeviceSensorId = ds.DeviceSensorId,
                    SensorId = ds.SensorId,
                    Name = ds.Sensor.Name,
                    IsEnabled = ds.IsEnabled
                }).ToList()
            }).ToListAsync();
        }

        public async Task<DeviceConfig> GetDeviceAsync(string deviceId)
        {
            return await configDb.Device
                .Where(d => d.DeviceId == Guid.Parse(deviceId))
                .Select(d => new DeviceConfig
                {
                    DeviceId = d.DeviceId.ToString(),
                    Name = d.Name,
                    Sensors = d.DeviceSensor.Select(ds => new DeviceSensorConfig
                    {
                        DeviceSensorId = ds.DeviceSensorId,
                        SensorId = ds.SensorId,
                        Name = ds.Sensor.Name,
                        IsEnabled = ds.IsEnabled
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
            catch (System.Exception)
            {
                //logger.LogError(new EventId(5001), ex, string.Empty);
                throw;
            }

            return await GetDeviceAsync(dbDevice.DeviceId.ToString());
        }

        public async Task<List<SensorConfig>> GetSensorListAsync()
        {
            return await configDb.Sensor
                .Select(s => new SensorConfig
                {
                    SensorId = s.SensorId,
                    Name = s.Name
                }).ToListAsync();
        }

        public async Task<SensorConfig> GetSensorAsync(int sensorId)
        {
            return await configDb.Sensor
                .Where(s => s.SensorId == sensorId)
                .Select(s => new SensorConfig
                {
                    SensorId = s.SensorId,
                    Name = s.Name
                }).FirstOrDefaultAsync();
        }

        public async Task<SensorConfig> SaveSensorAsync(SensorConfig sensor)
        {
            Sensor dbSensor = null;

            if (!sensor.SensorId.HasValue)
            {
                dbSensor = new Sensor();
                dbSensor.Name = sensor.Name;

                await configDb.Sensor.AddAsync(dbSensor);
            }
            else
            {
                dbSensor = await configDb.Sensor.FirstOrDefaultAsync(s => s.SensorId == sensor.SensorId.Value);

                if (dbSensor == null)
                {
                    throw new ApplicationException($"Device with id {sensor.SensorId.ToString()} was not found.");
                }

                dbSensor.Name = sensor.Name;

                configDb.Sensor.Update(dbSensor);
            }

            try
            {
                await configDb.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                //logger.LogError(new EventId(5001), ex, string.Empty);
                throw;
            }

            return await GetSensorAsync(dbSensor.SensorId);
        }
    }
}