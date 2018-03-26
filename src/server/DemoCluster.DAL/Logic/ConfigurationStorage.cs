using DemoCluster.DAL.Database;
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
        private readonly RuntimeContext configDb;
        public ConfigurationStorage(RuntimeContext context)
        {
            configDb = context;
        }

        public async Task<List<DeviceConfig>> GetDeviceListAsync()
        {
            return await configDb.Device.Select(d => new DeviceConfig
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

        public async Task<DeviceConfig> GetDeviceAsync(string deviceId)
        {
            return await configDb.Device
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
                dbDevice.IsEnabled = device.IsEnabled;

                configDb.Device.Update(dbDevice);
            }

            try
            {
                await configDb.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                throw;
            }

            configDb.DeviceSensor.RemoveRange(configDb.DeviceSensor.Where(ds => !device.Sensors.Any(s => s.DeviceSensorId == ds.DeviceSensorId)));
            await configDb.SaveChangesAsync();

            foreach (var sensor in device.Sensors)
            {
                DeviceSensor dbDeviceSensor = null;
                if (sensor.DeviceSensorId.HasValue)
                {
                    dbDeviceSensor = await configDb.DeviceSensor.FirstOrDefaultAsync(ds => ds.DeviceSensorId == sensor.DeviceSensorId);
                    dbDeviceSensor.IsEnabled = sensor.IsEnabled;
                    configDb.DeviceSensor.Update(dbDeviceSensor);
                }
                else
                {
                    dbDeviceSensor = new DeviceSensor();
                    dbDeviceSensor.DeviceId = dbDevice.DeviceId;
                    dbDeviceSensor.SensorId = sensor.SensorId;
                    dbDeviceSensor.IsEnabled = sensor.IsEnabled;
                    await configDb.DeviceSensor.AddAsync(dbDeviceSensor);
                }
            }

            try
            {
                await configDb.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return await GetDeviceAsync(dbDevice.DeviceId.ToString());
        }

        public async Task RemoveDeviceAsync(string deviceId)
        {
            Device device = await configDb.Device.FirstOrDefaultAsync(d => d.DeviceId == Guid.Parse(deviceId));
            if (device == null)
            {
                throw new ApplicationException($"Device {deviceId} was not found.");
            }

            configDb.DeviceSensor.RemoveRange(configDb.DeviceSensor.Where(ds => ds.DeviceId == device.DeviceId));
            await configDb.SaveChangesAsync();

            configDb.Device.Remove(device);
            await configDb.SaveChangesAsync();
        }

        public async Task<List<SensorConfig>> GetSensorListAsync()
        {
            return await configDb.Sensor
                .Select(s => new SensorConfig
                {
                    SensorId = s.SensorId,
                    Name = s.Name,
                    Uom = s.Uom
                }).ToListAsync();
        }

        public async Task<SensorConfig> GetSensorAsync(int sensorId)
        {
            return await configDb.Sensor
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
                dbSensor.Uom = sensor.Uom;

                configDb.Sensor.Update(dbSensor);
            }

            try
            {
                await configDb.SaveChangesAsync();
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
            Sensor sensor = await configDb.Sensor.FirstOrDefaultAsync(s => s.SensorId == sensorId);
            if (sensor == null)
            {
                throw new ApplicationException($"Sensor {sensorId} was not found.");
            }

            configDb.DeviceSensor.RemoveRange(configDb.DeviceSensor.Where(ds => ds.SensorId == sensor.SensorId));
            await configDb.SaveChangesAsync();

            configDb.Sensor.Remove(sensor);
            await configDb.SaveChangesAsync();
        }
    }
}