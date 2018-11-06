using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DemoCluster.DAL.Database.Configuration;
using DemoCluster.DAL.Models;
using DemoCluster.Repository;
using Microsoft.Extensions.Logging;

namespace DemoCluster.DAL.Logic
{
    public class DeviceLogic
    {
        private readonly ILogger logger;
        private readonly IRepository<Device, ConfigurationContext> devices;
        private readonly IRepository<DeviceSensor, ConfigurationContext> deviceSensors;
        private readonly IRepository<DeviceEventType, ConfigurationContext> deviceEvents;
        private readonly IRepository<DeviceState, ConfigurationContext> deviceStates;

        public DeviceLogic(ILogger<DeviceLogic> logger, 
            IRepository<Device, ConfigurationContext> devices,
            IRepository<DeviceSensor, ConfigurationContext> deviceSensors,
            IRepository<DeviceEventType, ConfigurationContext> deviceEvents,
            IRepository<DeviceState, ConfigurationContext> deviceStates)
        {
            this.logger = logger;
            this.devices = devices;
            this.deviceSensors = deviceSensors;
            this.deviceEvents = deviceEvents;
            this.deviceStates = deviceStates;
        }

        #region Device Configuration
        public async Task<DeviceConfig> CreateDeviceAsync(DeviceConfig model, CancellationToken token = default(CancellationToken))
        {
            DeviceConfig result = null;

            try
            {
                await SaveDeviceAsync(model);

                foreach (var deviceSensor in model.Sensors)
                {
                    await SaveDeviceSensorAsync(deviceSensor);
                }

                foreach (var deviceEvent in model.Events)
                {
                    await SaveDeviceEventAsync(deviceEvent);
                }

                foreach (var deviceState in model.States)
                {
                    await SaveDeviceStateAsync(deviceState);
                }

                result = await GetDeviceAsync(model.Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error creating device {model.Name}.");
                throw;
            }

            return result;
        }

        public async Task<DeviceConfig> UpdateDeviceAsync(DeviceConfig model, CancellationToken token = default(CancellationToken))
        {
            DeviceConfig config = await GetDeviceAsync(Guid.Parse(model.DeviceId));
            if (config == null)
            {
                logger.LogError($"Unable to find device {model.Name} with id {model.DeviceId}.");
                throw new ApplicationException($"Unable to find device {model.Name} with id {model.DeviceId}.");
            }

            try
            {
                await SaveDeviceAsync(model);

                foreach (var deviceSensor in model.Sensors)
                {
                    await SaveDeviceSensorAsync(deviceSensor);
                }

                foreach (var deviceEvent in model.Events)
                {
                    await SaveDeviceEventAsync(deviceEvent);
                }

                foreach (var deviceState in model.States)
                {
                    await SaveDeviceStateAsync(deviceState);
                }

                config = await GetDeviceAsync(Guid.Parse(model.DeviceId));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error updating device {model.Name}.");
                throw;
            }

            return config;
        }

        public async Task DeleteDeviceAsync(DeviceConfig model, CancellationToken token = default(CancellationToken))
        {
            DeviceConfig config = await GetDeviceAsync(Guid.Parse(model.DeviceId));
            if (config == null)
            {
                logger.LogError($"Unable to find device {model.Name} with id {model.DeviceId}.");
                throw new ApplicationException($"Unable to find device {model.Name} with id {model.DeviceId}.");
            }

            try
            {
                foreach (var deviceState in model.States)
                {
                    await RemoveDeviceStateAsync(deviceState);
                }

                foreach (var deviceEvent in model.Events)
                {
                    await RemoveDeviceEventAsync(deviceEvent);
                }

                foreach (var deviceSensor in model.Sensors)
                {
                    await RemoveDeviceSensorAsync(deviceSensor);
                }

                await RemoveDeviceAsync(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error removing device {model.Name}.");
                throw;
            }
        }
        #endregion

        #region Device CRUD
        public async Task<List<DeviceConfig>> GetDeviceListAsync(CancellationToken token = default(CancellationToken))
        {
            IEnumerable<Device> listResults = await devices.AllAsync(token, Constants.DEVICE_PROPERTIES);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<List<DeviceConfig>> GetDeviceListAsync(Expression<Func<Device, bool>> filter,
            CancellationToken token = default(CancellationToken))
        {
            IEnumerable<Device> listResults = await devices.FindByAsync(filter, token, Constants.DEVICE_PROPERTIES);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<List<DeviceConfig>> GetDeviceListAsync(Expression<Func<Device, bool>> filter,
            Func<IQueryable<Device>, IOrderedQueryable<Device>> orderBy,
            CancellationToken token = default(CancellationToken))
        {
            IEnumerable<Device> listResults = await devices.FindByAsync(filter, orderBy, token, Constants.DEVICE_PROPERTIES);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<PaginatedList<DeviceConfig>> GetDevicePageAsync(string filter, 
            int pageIndex, 
            int pageSize,
            CancellationToken token = default(CancellationToken))
        {
            IEnumerable<Device> listResults = await devices.AllAsync();

            return listResults
                .Where(s => string.IsNullOrEmpty(filter) || s.Name.Contains(filter))
                .Select(o => o.ToViewModel())
                .ToPaginatedList(pageIndex, pageSize);
        }

        public async Task<DeviceConfig> GetDeviceAsync(Guid deviceId,
            CancellationToken token = default(CancellationToken))
        {
            IEnumerable<Device> searchResults = await devices.FindByAsync(d => d.DeviceId == deviceId, token, Constants.DEVICE_PROPERTIES);
            Device result = searchResults.FirstOrDefault();

            return result?.ToViewModel();
        }

        public async Task<DeviceConfig> GetDeviceAsync(string deviceName,
            CancellationToken token = default(CancellationToken))
        {
            IEnumerable<Device> searchResults = await devices.FindByAsync(d => d.Name == deviceName, token, Constants.DEVICE_PROPERTIES);
            Device result = searchResults.FirstOrDefault();

            return result?.ToViewModel();
        }

        public async Task<DeviceConfig> SaveDeviceAsync(DeviceConfig model,
            CancellationToken token = default(CancellationToken))
        {
            Device deviceItem = null;

            try
            {
                RepositoryResult result = null;

                if (string.IsNullOrEmpty(model.DeviceId))
                {
                    result = await devices.CreateAsync(model.ToModel());
                }
                else
                {
                    Device original = await devices.FindByKeyAsync(Guid.Parse(model.DeviceId));
                    result = await devices.UpdateAsync(original, model.ToModel());
                }

                if (result.Succeeded)
                {
                    logger.LogInformation($"Created device {model.Name}");

                    deviceItem = await devices.FindByKeyAsync(model.Name);
                    if (deviceItem == null)
                    {
                        logger.LogError($"Unable to find device {model.Name} as result.");
                    }
                }
                else
                {
                    LogErrors(result.Errors);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error creating device {model.Name}");
                throw;
            }

            return deviceItem?.ToViewModel();
        }

        public async Task RemoveDeviceAsync(DeviceConfig model,
            CancellationToken token = default(CancellationToken))
        {
            try
            {
                RepositoryResult result = await devices.DeleteAsync(model.ToModel());

                if (result.Succeeded)
                {
                    logger.LogInformation($"Removed service {model.Name} successfully.");
                }
                else
                {
                    LogErrors(result.Errors);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error removing service {model.Name}");
                throw;
            }
        }
        #endregion

        #region Device Sensor CRUD
        public async Task<List<DeviceSensorConfig>> GetDeviceSensorListAsync(CancellationToken token = default(CancellationToken))
        {
            IEnumerable<DeviceSensor> listResults = await deviceSensors.AllAsync();

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<List<DeviceSensorConfig>> GetDeviceSensorListAsync(Expression<Func<DeviceSensor, bool>> filter,
            CancellationToken token = default(CancellationToken))
        {
            IEnumerable<DeviceSensor> listResults = await deviceSensors.FindByAsync(filter);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<List<DeviceSensorConfig>> GetDeviceSensorListAsync(Expression<Func<DeviceSensor, bool>> filter,
            Func<IQueryable<DeviceSensor>, IOrderedQueryable<DeviceSensor>> orderBy,
            CancellationToken token = default(CancellationToken))
        {
            IEnumerable<DeviceSensor> listResults = await deviceSensors.FindByAsync(filter, orderBy);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<PaginatedList<DeviceSensorConfig>> GetDeviceSensorPageAsync(Guid deviceId, 
            string filter,
            int pageIndex,
            int pageSize,
            CancellationToken token = default(CancellationToken))
        {
            Device device = await devices.FindByKeyAsync(deviceId);
            if (device == null)
            {
                logger.LogError($"Device Id {deviceId.ToString()} was not found.");
                return null;
            }

            IEnumerable<DeviceSensor> listResults = await deviceSensors.FindByAsync(s => s.DeviceId == deviceId);

            return listResults
                .Where(f => string.IsNullOrEmpty(filter) || f.Sensor.Name.Contains(filter))
                .Select(s => s.ToViewModel())
                .ToPaginatedList(pageIndex, pageSize);
        }

        public async Task<DeviceSensorConfig> SaveDeviceSensorAsync(DeviceSensorConfig model,
            CancellationToken token = default(CancellationToken))
        {
            DeviceSensor deviceSensorItem = null;

            try
            {
                RepositoryResult result = null;

                if (!model.DeviceSensorId.HasValue)
                {
                    result = await deviceSensors.CreateAsync(model.ToModel());
                }
                else
                {
                    DeviceSensor original = await deviceSensors.FindByKeyAsync(model.DeviceSensorId.Value);
                    result = await deviceSensors.UpdateAsync(original, model.ToModel());
                }

                if (result.Succeeded)
                {
                    logger.LogInformation($"Created device sensor {model.Name}");

                    IEnumerable<DeviceSensor> searchResults = await deviceSensors.FindByAsync(s => s.Sensor.Name == model.Name);
                    deviceSensorItem = searchResults.FirstOrDefault();
                    if (deviceSensorItem == null)
                    {
                        logger.LogError($"Unable to find device sensor {model.Name} as result.");
                    }
                }
                else
                {
                    LogErrors(result.Errors);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error creating device {model.Name}");
                throw;
            }

            return deviceSensorItem?.ToViewModel();
        }

        public async Task RemoveDeviceSensorAsync(DeviceSensorConfig model,
            CancellationToken token = default(CancellationToken))
        {
            try
            {
                RepositoryResult result = await deviceSensors.DeleteAsync(model.ToModel());

                if (result.Succeeded)
                {
                    logger.LogInformation($"Removed service {model.Name} successfully.");
                }
                else
                {
                    LogErrors(result.Errors);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error removing service {model.Name}");
                throw;
            }
        }
        #endregion

        #region Device Event CRUD
        public async Task<List<DeviceEventConfig>> GetDeviceEventListAsync(CancellationToken token = default(CancellationToken))
        {
            IEnumerable<DeviceEventType> listResults = await deviceEvents.AllAsync(token);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<List<DeviceEventConfig>> GetDeviceEventListAsync(Expression<Func<DeviceEventType, bool>> filter,
            CancellationToken token = default(CancellationToken))
        {
            IEnumerable<DeviceEventType> listResults = await deviceEvents.FindByAsync(filter);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<List<DeviceEventConfig>> GetDeviceEventListAsync(Expression<Func<DeviceEventType, bool>> filter,
            Func<IQueryable<DeviceEventType>, IOrderedQueryable<DeviceEventType>> orderBy,
            CancellationToken token = default(CancellationToken))
        {
            IEnumerable<DeviceEventType> listResults = await deviceEvents.FindByAsync(filter, orderBy);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<PaginatedList<DeviceEventConfig>> GetDeviceEventPageAsync(Guid deviceId, 
            string filter,
            int pageIndex,
            int pageSize,
            CancellationToken token = default(CancellationToken))
        {
            Device device = await devices.FindByKeyAsync(deviceId);
            if (device == null)
            {
                logger.LogError($"Device Id {deviceId.ToString()} was not found.");
                return null;
            }

            IEnumerable<DeviceEventType> listResults = await deviceEvents.FindByAsync(s => s.DeviceId == deviceId);

            return listResults
                .Where(f => string.IsNullOrEmpty(filter) || f.EventType.Name.Contains(filter))
                .Select(s => s.ToViewModel())
                .ToPaginatedList(pageIndex, pageSize);
        }

        public async Task<DeviceEventConfig> SaveDeviceEventAsync(DeviceEventConfig model,
            CancellationToken token = default(CancellationToken))
        {
            DeviceEventType deviceEventItem = null;

            try
            {
                RepositoryResult result = null;

                if (!model.DeviceEventTypeId.HasValue)
                {
                    result = await deviceEvents.CreateAsync(model.ToModel());
                }
                else
                {
                    DeviceEventType original = await deviceEvents.FindByKeyAsync(model.DeviceEventTypeId.Value);
                    result = await deviceEvents.UpdateAsync(original, model.ToModel());
                }

                if (result.Succeeded)
                {
                    logger.LogInformation($"Created device sensor {model.Name}");

                    IEnumerable<DeviceEventType> searchResults = await deviceEvents.FindByAsync(s => s.EventType.Name == model.Name);
                    deviceEventItem = searchResults.FirstOrDefault();
                    if (deviceEventItem == null)
                    {
                        logger.LogError($"Unable to find device sensor {model.Name} as result.");
                    }
                }
                else
                {
                    LogErrors(result.Errors);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error creating device {model.Name}");
                throw;
            }

            return deviceEventItem?.ToViewModel();
        }

        public async Task RemoveDeviceEventAsync(DeviceEventConfig model,
            CancellationToken token = default(CancellationToken))
        {
            try
            {
                RepositoryResult result = await deviceEvents.DeleteAsync(model.ToModel());

                if (result.Succeeded)
                {
                    logger.LogInformation($"Removed service {model.Name} successfully.");
                }
                else
                {
                    LogErrors(result.Errors);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error removing service {model.Name}");
                throw;
            }
        }
        #endregion

        #region Device State CRUD
        public async Task<List<DeviceStateConfig>> GetDeviceStateListAsync(CancellationToken token = default(CancellationToken))
        {
            IEnumerable<DeviceState> listResults = await deviceStates.AllAsync(token);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<List<DeviceStateConfig>> GetDeviceStateListAsync(Expression<Func<DeviceState, bool>> filter,
            CancellationToken token = default(CancellationToken))
        {
            IEnumerable<DeviceState> listResults = await deviceStates.FindByAsync(filter);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<List<DeviceStateConfig>> GetDeviceStateListAsync(Expression<Func<DeviceState, bool>> filter,
            Func<IQueryable<DeviceState>, IOrderedQueryable<DeviceState>> orderBy,
            CancellationToken token = default(CancellationToken))
        {
            IEnumerable<DeviceState> listResults = await deviceStates.FindByAsync(filter, orderBy);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<PaginatedList<DeviceStateConfig>> GetDeviceStatePageAsync(Guid deviceId, 
            string filter,
            int pageIndex,
            int pageSize,
            CancellationToken token = default(CancellationToken))
        {
            Device device = await devices.FindByKeyAsync(deviceId);
            if (device == null)
            {
                logger.LogError($"Device Id {deviceId.ToString()} was not found.");
                return null;
            }

            IEnumerable<DeviceState> listResults = await deviceStates.FindByAsync(s => s.DeviceId == deviceId);

            return listResults
                .Where(f => string.IsNullOrEmpty(filter) || f.State.Name.Contains(filter))
                .Select(s => s.ToViewModel())
                .ToPaginatedList(pageIndex, pageSize);
        }

        public async Task<DeviceStateConfig> SaveDeviceStateAsync(DeviceStateConfig model,
            CancellationToken token = default(CancellationToken))
        {
            DeviceState deviceStateItem = null;

            try
            {
                RepositoryResult result = null;

                if (!model.DeviceStateId.HasValue)
                {
                    result = await deviceStates.CreateAsync(model.ToModel());
                }
                else
                {
                    DeviceState original = await deviceStates.FindByKeyAsync(model.DeviceStateId.Value);
                    result = await deviceStates.UpdateAsync(original, model.ToModel());
                }

                if (result.Succeeded)
                {
                    logger.LogInformation($"Created device sensor {model.Name}");

                    IEnumerable<DeviceState> searchResults = await deviceStates.FindByAsync(s => s.State.Name == model.Name);
                    deviceStateItem = searchResults.FirstOrDefault();
                    if (deviceStateItem == null)
                    {
                        logger.LogError($"Unable to find device sensor {model.Name} as result.");
                    }
                }
                else
                {
                    LogErrors(result.Errors);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error creating device {model.Name}");
                throw;
            }

            return deviceStateItem?.ToViewModel();
        }

        public async Task RemoveDeviceStateAsync(DeviceStateConfig model,
            CancellationToken token = default(CancellationToken))
        {
            try
            {
                RepositoryResult result = await deviceStates.DeleteAsync(model.ToModel());

                if (result.Succeeded)
                {
                    logger.LogInformation($"Removed service {model.Name} successfully.");
                }
                else
                {
                    LogErrors(result.Errors);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error removing service {model.Name}");
                throw;
            }
        }
        #endregion

        private void LogErrors(IEnumerable<RepositoryError> errors)
        {
            foreach (var error in errors)
            {
                logger.LogError($"{error.Code} - {error.Description}");
            }
        }
    }
}