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
                }).ToList(),
                Events = d.DeviceEventType.Select(de => new DeviceEventConfig
                {
                    DeviceEventTypeId = de.DeviceEventTypeId,
                    EventTypeId = de.EventTypeId,
                    Name = de.EventType.Name,
                    IsEnabled = de.IsEnabled
                }).ToList(),
                States = d.DeviceState.Select(ds => new DeviceStateConfig
                {
                    DeviceStateId = ds.DeviceStateId,
                    StateId = ds.StateId,
                    Name = ds.State.Name,
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
                    }).ToList(),
                    Events = d.DeviceEventType.Select(de => new DeviceEventConfig
                    {
                        DeviceEventTypeId = de.DeviceEventTypeId,
                        EventTypeId = de.EventTypeId,
                        Name = de.EventType.Name,
                        IsEnabled = de.IsEnabled
                    }).ToList(),
                    States = d.DeviceState.Select(ds => new DeviceStateConfig
                    {
                        DeviceStateId = ds.DeviceStateId,
                        StateId = ds.StateId,
                        Name = ds.State.Name,
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
                    }).ToList(),
                    Events = d.DeviceEventType.Select(de => new DeviceEventConfig
                    {
                        DeviceEventTypeId = de.DeviceEventTypeId,
                        EventTypeId = de.EventTypeId,
                        Name = de.EventType.Name,
                        IsEnabled = de.IsEnabled
                    }).ToList(),
                    States = d.DeviceState.Select(ds => new DeviceStateConfig
                    {
                        DeviceStateId = ds.DeviceStateId,
                        StateId = ds.StateId,
                        Name = ds.State.Name,
                        IsEnabled = ds.IsEnabled
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<DeviceConfig> SaveDeviceAsync(DeviceConfig model)
        {
            Device dbDevice = null;

            var transaction = await context.Database.BeginTransactionAsync();

            if (string.IsNullOrEmpty(model.DeviceId))
            {
                dbDevice = new Device
                {
                    Name = model.Name,
                    IsEnabled = model.IsEnabled
                };

                await context.Device.AddAsync(dbDevice);
            }
            else
            {
                dbDevice = await context.Device.FirstOrDefaultAsync(d => d.DeviceId == Guid.Parse(model.DeviceId));

                if (dbDevice == null)
                {
                    throw new ApplicationException($"Device with id {model.DeviceId.ToString()} was not found.");
                }

                dbDevice.Name = model.Name;
                dbDevice.IsEnabled = model.IsEnabled;

                context.Device.Update(dbDevice);
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                transaction.Rollback();
                throw;
            }

            context.DeviceSensor.RemoveRange(context.DeviceSensor.Where(ds => !model.Sensors.Any(s => s.DeviceSensorId == ds.DeviceSensorId)));
            await context.SaveChangesAsync();

            foreach (var sensor in model.Sensors)
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

            context.DeviceEventType.RemoveRange(context.DeviceEventType.Where(de => !model.Events.Any(e => e.DeviceEventTypeId == de.DeviceEventTypeId)));
            await context.SaveChangesAsync();

            foreach (var eventType in model.Events)
            {
                DeviceEventType dbDeviceEventType = null;
                if (eventType.DeviceEventTypeId.HasValue)
                {
                    dbDeviceEventType = await context.DeviceEventType.FirstOrDefaultAsync(de => de.DeviceEventTypeId == eventType.DeviceEventTypeId);
                    dbDeviceEventType.IsEnabled = eventType.IsEnabled;
                    context.DeviceEventType.Update(dbDeviceEventType);
                }
                else
                {
                    dbDeviceEventType = new DeviceEventType();
                    dbDeviceEventType.DeviceId = dbDevice.DeviceId;
                    dbDeviceEventType.EventTypeId = eventType.EventTypeId;
                    dbDeviceEventType.IsEnabled = eventType.IsEnabled;
                    await context.DeviceEventType.AddAsync(dbDeviceEventType);                    
                }
            }

            context.DeviceState.RemoveRange(context.DeviceState.Where(ds => !model.States.Any(e => e.DeviceStateId == ds.DeviceStateId)));
            await context.SaveChangesAsync();

            foreach (var state in model.States)
            {
                DeviceState dbDeviceState = null;
                if (state.DeviceStateId.HasValue)
                {
                    dbDeviceState = await context.DeviceState.FirstOrDefaultAsync(ds => ds.DeviceStateId == state.DeviceStateId);
                    dbDeviceState.IsEnabled = state.IsEnabled;
                    context.DeviceState.Update(dbDeviceState);
                }
                else
                {
                    dbDeviceState = new DeviceState();
                    dbDeviceState.DeviceId = dbDevice.DeviceId;
                    dbDeviceState.StateId = state.StateId;
                    dbDeviceState.IsEnabled = state.IsEnabled;
                    await context.DeviceState.AddAsync(dbDeviceState);                    
                }
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }

            transaction.Commit();

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

        public async Task<SensorConfig> SaveSensorAsync(SensorConfig model)
        {
            Sensor dbSensor = null;

            if (!model.SensorId.HasValue)
            {
                dbSensor = new Sensor
                {
                    Name = model.Name,
                    Uom = model.Uom
                };

                await context.Sensor.AddAsync(dbSensor);
            }
            else
            {
                dbSensor = await context.Sensor.FirstOrDefaultAsync(s => s.SensorId == model.SensorId.Value);

                if (dbSensor == null)
                {
                    throw new ApplicationException($"Sensor with id {model.SensorId.ToString()} was not found.");
                }

                dbSensor.Name = model.Name;
                dbSensor.Uom = model.Uom;

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

        public async Task<SensorDeviceConfig> GetDeviceSensorAsync(int deviceSensorId)
        {
            return await context.DeviceSensor
                .Where(ds => ds.DeviceSensorId == deviceSensorId)
                .Select(ds => new SensorDeviceConfig
                {
                    DeviceSensorId = ds.DeviceSensorId,
                    DeviceName = ds.Device.Name,
                    SensorName = ds.Sensor.Name,
                    Uom = ds.Sensor.Uom,
                    IsEnabled = ds.IsEnabled
                }).FirstOrDefaultAsync();
        }

        public async Task<List<EventConfig>> GetEventListAsync(string sort, int index, string search)
        {
            var baseList = context.EventType.AsQueryable();

            if (!string.IsNullOrEmpty(search)) 
            {
                baseList = baseList.Where(e => e.Name.ToLower().Contains(search.ToLower()));
            }

            switch (sort)
            {
                case "name_desc":
                    baseList = baseList.OrderByDescending(e => e.Name);
                    break;
                default:
                    baseList = baseList.OrderBy(e => e.Name);
                    break;
            }

            return await baseList.AsNoTracking()
                .Select(e => new EventConfig
                {
                    EventId = e.EventTypeId,
                    Name = e.Name 
                }).ToListAsync();
        }

        public async Task<EventConfig> GetEventAsync(int eventId)
        {
            return await context.EventType
                .Where(e => e.EventTypeId == eventId)
                .Select(e => new EventConfig
                {
                    EventId = e.EventTypeId,
                    Name = e.Name
                }).FirstOrDefaultAsync();
        }

        public async Task<EventConfig> SaveEventAsync(EventConfig model)
        {
            EventType dbEvent = null;

            if (!model.EventId.HasValue)
            {
                dbEvent = new EventType
                {
                    Name = model.Name
                };

                await context.EventType.AddAsync(dbEvent);
            }
            else
            {
                dbEvent = await context.EventType.FirstOrDefaultAsync(e => e.EventTypeId == model.EventId);
                if (dbEvent == null)
                {
                    throw new ApplicationException($"Event with id {model.EventId} was not found.");
                }

                dbEvent.Name = model.Name;

                context.EventType.Update(dbEvent);
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return await GetEventAsync(dbEvent.EventTypeId);
        }

        public async Task RemoveEventAsync(int eventId)
        {
            EventType dbEvent = await context.EventType.FirstOrDefaultAsync(e => e.EventTypeId == eventId);
            if (dbEvent == null)
            {
                throw new ApplicationException($"Event with {eventId} was not found.");
            }

            context.DeviceEventType.RemoveRange(context.DeviceEventType.Where(de => de.EventTypeId == dbEvent.EventTypeId));
            await context.SaveChangesAsync();

            context.EventType.Remove(dbEvent);
            await context.SaveChangesAsync();
        }

        public async Task<List<StateConfig>> GetStateListAsync(string sort, int index, string search)
        {
            var baseList = context.State.AsQueryable();

            if (!string.IsNullOrEmpty(search)) 
            {
                baseList = baseList.Where(s => s.Name.ToLower().Contains(search.ToLower()));
            }

            switch (sort)
            {
                case "name_desc":
                    baseList = baseList.OrderByDescending(s => s.Name);
                    break;
                default:
                    baseList = baseList.OrderBy(s => s.Name);
                    break;
            }

            return await baseList.AsNoTracking()
                .Select(s => new StateConfig
                {
                    StateId = s.StateId,
                    Name = s.Name
                }).ToListAsync();
        }

        public async Task<StateConfig> GetStateAsync(int stateId)
        {
            return await context.State
                .Where(s => s.StateId == stateId)
                .Select(s => new StateConfig
                {
                    StateId = s.StateId,
                    Name = s.Name
                }).FirstOrDefaultAsync();
        }

        public async Task<StateConfig> SaveStateAsync(StateConfig model)
        {
            State dbState = null;

            if (!model.StateId.HasValue)
            {
                dbState = new State
                {
                    Name = model.Name
                };

                await context.State.AddAsync(dbState);
            }
            else
            {
                dbState = await context.State.FirstOrDefaultAsync(s => s.StateId == model.StateId.Value);

                if (dbState == null)
                {
                    throw new ApplicationException($"State with id {model.StateId.ToString()} was not found.");
                }

                dbState.Name = model.Name;

                context.State.Update(dbState);
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

            return await GetStateAsync(dbState.StateId);
        }

        public async Task RemoveStateAsync(int stateId)
        {
            State state = await context.State.FirstOrDefaultAsync(s => s.StateId == stateId);
            if (state == null)
            {
                throw new ApplicationException($"State {stateId} was not found.");
            }

            context.DeviceState.RemoveRange(context.DeviceState.Where(ds => ds.StateId == state.StateId));
            await context.SaveChangesAsync();

            context.State.Remove(state);
            await context.SaveChangesAsync();
        }
    }
}