using DemoCluster.DAL.Database;
using DemoCluster.DAL.Database.Configuration;
using DemoCluster.DAL.Models;
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
        private readonly ConfigurationContext context;
        public ConfigurationStorage(ConfigurationContext context)
        {
            this.context = context;
        }

        public async Task<List<DeviceConfig>> GetDeviceListAsync()
        {
            return await context.Device.Select(d => new DeviceConfig
            {
                DeviceId = d.DeviceId.ToString(),
                Name = d.Name,
                IsEnabled = d.IsEnabled,
                Sensors = d.DeviceSensor.Select(ds => new DeviceSensorConfig
                {
                    DeviceSensorId = ds.DeviceSensorId,
                    SensorId = ds.SensorId,
                    Name = ds.Sensor.Name,
                    UOM = ds.Sensor.Uom,
                    IsEnabled = ds.IsEnabled
                }).ToList()
            }).ToListAsync();
        }

        public async Task<DeviceConfig> GetDeviceByIdAsync(string deviceId)
        {
            return await context.Device
                .Where(d => d.DeviceId == Guid.Parse(deviceId))
                .Select(d => new DeviceConfig
                {
                    DeviceId = d.DeviceId.ToString(),
                    Name = d.Name,
                    IsEnabled = d.IsEnabled,
                    Sensors = d.DeviceSensor.Select(ds => new DeviceSensorConfig
                    {
                        DeviceSensorId = ds.DeviceSensorId,
                        SensorId = ds.SensorId,
                        Name = ds.Sensor.Name,
                        UOM = ds.Sensor.Uom,
                        IsEnabled = ds.IsEnabled
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<DeviceConfig> GetDeviceByNameAsync(string name)
        {
            return await context.Device
                .Where(d => d.Name == name)
                .Select(d => new DeviceConfig
                {
                    DeviceId = d.DeviceId.ToString(),
                    Name = d.Name,
                    IsEnabled = d.IsEnabled,
                    Sensors = d.DeviceSensor.Select(ds => new DeviceSensorConfig
                    {
                        DeviceSensorId = ds.DeviceSensorId,
                        SensorId = ds.SensorId,
                        Name = ds.Sensor.Name,
                        UOM = ds.Sensor.Uom,
                        IsEnabled = ds.IsEnabled
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<DeviceConfig> SaveDeviceAsync(DeviceConfig device)
        {
            Device dbDevice = null;

            if (string.IsNullOrEmpty(device.DeviceId))
            {
                dbDevice = new Device
                {
                    Name = device.Name,
                    IsEnabled = device.IsEnabled
                };

                await context.Device.AddAsync(dbDevice);
            }
            else
            {
                dbDevice = await context.Device.FirstOrDefaultAsync(d => d.DeviceId == Guid.Parse(device.DeviceId));

                if (dbDevice == null)
                {
                    throw new ApplicationException($"Device with id {device.DeviceId.ToString()} was not found.");
                }

                dbDevice.Name = device.Name;
                dbDevice.IsEnabled = device.IsEnabled;

                context.Device.Update(dbDevice);
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                throw;
            }

            context.DeviceSensor.RemoveRange(context.DeviceSensor.Where(ds => !device.Sensors.Any(s => s.DeviceSensorId == ds.DeviceSensorId)));
            await context.SaveChangesAsync();

            foreach (var sensor in device.Sensors)
            {
                DeviceSensor dbDeviceSensor = null;
                if (sensor.DeviceSensorId.HasValue)
                {
                    dbDeviceSensor = await context.DeviceSensor.FirstOrDefaultAsync(ds => ds.DeviceSensorId == sensor.DeviceSensorId);
                    dbDeviceSensor.IsEnabled = sensor.IsEnabled;
                    context.DeviceSensor.Update(dbDeviceSensor);
                }
                else
                {
                    dbDeviceSensor = new DeviceSensor();
                    dbDeviceSensor.DeviceId = dbDevice.DeviceId;
                    dbDeviceSensor.SensorId = sensor.SensorId;
                    dbDeviceSensor.IsEnabled = sensor.IsEnabled;
                    await context.DeviceSensor.AddAsync(dbDeviceSensor);
                }
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return await GetDeviceByIdAsync(dbDevice.DeviceId.ToString());
        }

        public async Task RemoveDeviceAsync(string deviceId)
        {
            Device device = await context.Device.FirstOrDefaultAsync(d => d.DeviceId == Guid.Parse(deviceId));
            if (device == null)
            {
                throw new ApplicationException($"Device {deviceId} was not found.");
            }

            context.DeviceSensor.RemoveRange(context.DeviceSensor.Where(ds => ds.DeviceId == device.DeviceId));
            await context.SaveChangesAsync();

            context.Device.Remove(device);
            await context.SaveChangesAsync();
        }

        public async Task<List<SensorConfig>> GetSensorListAsync(string sort, int index, string search)
        {
            var baseList = context.Sensor.AsQueryable();

            if (!string.IsNullOrEmpty(search)) 
            {
                baseList = baseList.Where(s => s.Name.ToLower().Contains(search.ToLower()));
            }

            switch (sort)
            {
                case "uom_asc":
                    baseList = baseList.OrderBy(s => s.Uom);
                    break;
                case "uom_desc":
                    baseList = baseList.OrderByDescending(s => s.Uom);
                    break;
                case "name_desc":
                    baseList = baseList.OrderByDescending(s => s.Name);
                    break;
                default:
                    baseList = baseList.OrderBy(s => s.Name);
                    break;
            }

            return await baseList.AsNoTracking()
                .Select(s => new SensorConfig
                {
                    SensorId = s.SensorId,
                    Name = s.Name,
                    Uom = s.Uom
                }).ToListAsync();
        }

        public async Task<SensorConfig> GetSensorAsync(int sensorId)
        {
            return await context.Sensor
                .Where(s => s.SensorId == sensorId)
                .Select(s => new SensorConfig
                {
                    SensorId = s.SensorId,
                    Name = s.Name,
                    Uom = s.Uom
                }).FirstOrDefaultAsync();
        }

        public async Task<SensorConfig> SaveSensorAsync(SensorConfig sensor)
        {
            Sensor dbSensor = null;

            if (!sensor.SensorId.HasValue)
            {
                dbSensor = new Sensor
                {
                    Name = sensor.Name,
                    Uom = sensor.Uom
                };

                await context.Sensor.AddAsync(dbSensor);
            }
            else
            {
                dbSensor = await context.Sensor.FirstOrDefaultAsync(s => s.SensorId == sensor.SensorId.Value);

                if (dbSensor == null)
                {
                    throw new ApplicationException($"Device with id {sensor.SensorId.ToString()} was not found.");
                }

                dbSensor.Name = sensor.Name;
                dbSensor.Uom = sensor.Uom;

                context.Sensor.Update(dbSensor);
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                //logger.LogError(new EventId(5001), ex, string.Empty);
                throw;
            }

            return await GetSensorAsync(dbSensor.SensorId);
        }

        public async Task RemoveSensorAsync(int sensorId)
        {
            Sensor sensor = await context.Sensor.FirstOrDefaultAsync(s => s.SensorId == sensorId);
            if (sensor == null)
            {
                throw new ApplicationException($"Sensor {sensorId} was not found.");
            }

            context.DeviceSensor.RemoveRange(context.DeviceSensor.Where(ds => ds.SensorId == sensor.SensorId));
            await context.SaveChangesAsync();

            context.Sensor.Remove(sensor);
            await context.SaveChangesAsync();
        }
    }
}