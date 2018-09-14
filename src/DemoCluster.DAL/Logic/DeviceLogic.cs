using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DemoCluster.DAL.Database.Configuration;
using DemoCluster.DAL.Models;
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
        public async Task<DeviceConfig> CreateDeviceAsync(DeviceConfig model, CancellationToken token)
        {
            DeviceConfig result = null;

            try
            {
                await SaveDeviceAsync(model, token);

                foreach (var deviceSensor in model.Sensors)
                {
                    await SaveDeviceSensorAsync(deviceSensor, token);
                }

                foreach (var deviceEvent in model.Events)
                {
                    await SaveDeviceEventAsync(deviceEvent, token);
                }

                foreach (var deviceState in model.States)
                {
                    await SaveDeviceStateAsync(deviceState, token);
                }

                result = await GetDeviceAsync(model.Name, token);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error creating device {model.Name}.");
                throw;
            }

            return result;
        }

        public async Task<DeviceConfig> UpdateDeviceAsync(DeviceConfig model, CancellationToken token)
        {
            DeviceConfig config = await GetDeviceAsync(Guid.Parse(model.DeviceId), token);
            if (config == null)
            {
                logger.LogError($"Unable to find device {model.Name} with id {model.DeviceId}.");
                throw new ApplicationException($"Unable to find device {model.Name} with id {model.DeviceId}.");
            }

            try
            {
                await SaveDeviceAsync(model, token);

                foreach (var deviceSensor in model.Sensors)
                {
                    await SaveDeviceSensorAsync(deviceSensor, token);
                }

                foreach (var deviceEvent in model.Events)
                {
                    await SaveDeviceEventAsync(deviceEvent, token);
                }

                foreach (var deviceState in model.States)
                {
                    await SaveDeviceStateAsync(deviceState, token);
                }

                config = await GetDeviceAsync(Guid.Parse(model.DeviceId), token);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error updating device {model.Name}.");
                throw;
            }

            return config;
        }

        public async Task DeleteDeviceAsync(DeviceConfig model, CancellationToken token)
        {
            DeviceConfig config = await GetDeviceAsync(Guid.Parse(model.DeviceId), token);
            if (config == null)
            {
                logger.LogError($"Unable to find device {model.Name} with id {model.DeviceId}.");
                throw new ApplicationException($"Unable to find device {model.Name} with id {model.DeviceId}.");
            }

            try
            {
                foreach (var deviceState in model.States)
                {
                    await RemoveDeviceStateAsync(deviceState, token);
                }

                foreach (var deviceEvent in model.Events)
                {
                    await RemoveDeviceEventAsync(deviceEvent, token);
                }

                foreach (var deviceSensor in model.Sensors)
                {
                    await RemoveDeviceSensorAsync(deviceSensor, token);
                }

                await RemoveDeviceAsync(model, token);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error removing device {model.Name}.");
                throw;
            }
        }
        #endregion

        #region Device CRUD
        public async Task<List<DeviceConfig>> GetDeviceListAsync(CancellationToken token)
        {
            IEnumerable<Device> listResults = await devices.AllAsync(token);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<List<DeviceConfig>> GetDeviceListAsync(Expression<Func<Device, bool>> filter,
            CancellationToken token)
        {
            IEnumerable<Device> listResults = await devices.FindByAsync(filter, token);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<List<DeviceConfig>> GetDeviceListAsync(Expression<Func<Device, bool>> filter,
            Func<IQueryable<Device>, IOrderedQueryable<Device>> orderBy,
            CancellationToken token)
        {
            IEnumerable<Device> listResults = await devices.FindByAsync(filter, orderBy, token);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<PaginatedList<DeviceConfig>> GetDevicePageAsync(string filter, 
            int pageIndex, 
            int pageSize,
            CancellationToken token)
        {
            IEnumerable<Device> listResults = await devices.AllAsync(token);

            return listResults
                .Where(s => string.IsNullOrEmpty(filter) || s.Name.Contains(filter))
                .Select(o => o.ToViewModel())
                .ToPaginatedList(pageIndex, pageSize);
        }

        public async Task<DeviceConfig> GetDeviceAsync(Guid deviceId,
            CancellationToken token)
        {
            Device result = await devices.FindByKeyAsync(deviceId, token);

            return result?.ToViewModel();
        }

        public async Task<DeviceConfig> GetDeviceAsync(string deviceName,
            CancellationToken token)
        {
            Device result = await devices.FindByKeyAsync(deviceName, token);

            return result?.ToViewModel();
        }

        public async Task<DeviceConfig> SaveDeviceAsync(DeviceConfig model,
            CancellationToken token)
        {
            Device deviceItem = null;

            try
            {
                RepositoryResult result = null;

                if (string.IsNullOrEmpty(model.DeviceId))
                {
                    result = await devices.CreateAsync(model.ToModel(), token);
                }
                else
                {
                    result = await devices.UpdateAsync(model.ToModel(), token);
                }

                if (result.Succeeded)
                {
                    logger.LogInformation($"Created device {model.Name}");

                    deviceItem = await devices.FindByKeyAsync(model.Name, token);
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
            CancellationToken token)
        {
            try
            {
                RepositoryResult result = await devices.DeleteAsync(model.ToModel(), token);

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
        public async Task<List<DeviceSensorConfig>> GetDeviceSensorListAsync(CancellationToken token)
        {
            IEnumerable<DeviceSensor> listResults = await deviceSensors.AllAsync(token);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<List<DeviceSensorConfig>> GetDeviceSensorListAsync(Expression<Func<DeviceSensor, bool>> filter,
            CancellationToken token)
        {
            IEnumerable<DeviceSensor> listResults = await deviceSensors.FindByAsync(filter, token);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<List<DeviceSensorConfig>> GetDeviceSensorListAsync(Expression<Func<DeviceSensor, bool>> filter,
            Func<IQueryable<DeviceSensor>, IOrderedQueryable<DeviceSensor>> orderBy,
            CancellationToken token)
        {
            IEnumerable<DeviceSensor> listResults = await deviceSensors.FindByAsync(filter, orderBy, token);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<PaginatedList<DeviceSensorConfig>> GetDeviceSensorPageAsync(Guid deviceId, 
            string filter,
            int pageIndex,
            int pageSize,
            CancellationToken token)
        {
            Device device = await devices.FindByKeyAsync(deviceId, token);
            if (device == null)
            {
                logger.LogError($"Device Id {deviceId.ToString()} was not found.");
                return null;
            }

            IEnumerable<DeviceSensor> listResults = await deviceSensors.FindByAsync(s => s.DeviceId == deviceId, token);

            return listResults
                .Where(f => string.IsNullOrEmpty(filter) || f.Sensor.Name.Contains(filter))
                .Select(s => s.ToViewModel())
                .ToPaginatedList(pageIndex, pageSize);
        }

        public async Task<DeviceSensorConfig> SaveDeviceSensorAsync(DeviceSensorConfig model,
            CancellationToken token)
        {
            DeviceSensor deviceSensorItem = null;

            try
            {
                RepositoryResult result = null;

                if (!model.DeviceSensorId.HasValue)
                {
                    result = await deviceSensors.CreateAsync(model.ToModel(), token);
                }
                else
                {
                    result = await deviceSensors.UpdateAsync(model.ToModel(), token);
                }

                if (result.Succeeded)
                {
                    logger.LogInformation($"Created device sensor {model.Name}");

                    IEnumerable<DeviceSensor> searchResults = await deviceSensors.FindByAsync(s => s.Sensor.Name == model.Name, token);
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
            CancellationToken token)
        {
            try
            {
                RepositoryResult result = await deviceSensors.DeleteAsync(model.ToModel(), token);

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
        public async Task<List<DeviceEventConfig>> GetDeviceEventListAsync(CancellationToken token)
        {
            IEnumerable<DeviceEventType> listResults = await deviceEvents.AllAsync(token);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<List<DeviceEventConfig>> GetDeviceEventListAsync(Expression<Func<DeviceEventType, bool>> filter,
            CancellationToken token)
        {
            IEnumerable<DeviceEventType> listResults = await deviceEvents.FindByAsync(filter, token);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<List<DeviceEventConfig>> GetDeviceEventListAsync(Expression<Func<DeviceEventType, bool>> filter,
            Func<IQueryable<DeviceEventType>, IOrderedQueryable<DeviceEventType>> orderBy,
            CancellationToken token)
        {
            IEnumerable<DeviceEventType> listResults = await deviceEvents.FindByAsync(filter, orderBy, token);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<PaginatedList<DeviceEventConfig>> GetDeviceEventPageAsync(Guid deviceId, 
            string filter,
            int pageIndex,
            int pageSize,
            CancellationToken token)
        {
            Device device = await devices.FindByKeyAsync(deviceId, token);
            if (device == null)
            {
                logger.LogError($"Device Id {deviceId.ToString()} was not found.");
                return null;
            }

            IEnumerable<DeviceEventType> listResults = await deviceEvents.FindByAsync(s => s.DeviceId == deviceId, token);

            return listResults
                .Where(f => string.IsNullOrEmpty(filter) || f.EventType.Name.Contains(filter))
                .Select(s => s.ToViewModel())
                .ToPaginatedList(pageIndex, pageSize);
        }

        public async Task<DeviceEventConfig> SaveDeviceEventAsync(DeviceEventConfig model,
            CancellationToken token)
        {
            DeviceEventType deviceEventItem = null;

            try
            {
                RepositoryResult result = null;

                if (!model.DeviceEventTypeId.HasValue)
                {
                    result = await deviceEvents.CreateAsync(model.ToModel(), token);
                }
                else
                {
                    result = await deviceEvents.UpdateAsync(model.ToModel(), token);
                }

                if (result.Succeeded)
                {
                    logger.LogInformation($"Created device sensor {model.Name}");

                    IEnumerable<DeviceEventType> searchResults = await deviceEvents.FindByAsync(s => s.EventType.Name == model.Name, token);
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
            CancellationToken token)
        {
            try
            {
                RepositoryResult result = await deviceEvents.DeleteAsync(model.ToModel(), token);

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
        public async Task<List<DeviceStateConfig>> GetDeviceStateListAsync(CancellationToken token)
        {
            IEnumerable<DeviceState> listResults = await deviceStates.AllAsync(token);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<List<DeviceStateConfig>> GetDeviceStateListAsync(Expression<Func<DeviceState, bool>> filter,
            CancellationToken token)
        {
            IEnumerable<DeviceState> listResults = await deviceStates.FindByAsync(filter, token);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<List<DeviceStateConfig>> GetDeviceStateListAsync(Expression<Func<DeviceState, bool>> filter,
            Func<IQueryable<DeviceState>, IOrderedQueryable<DeviceState>> orderBy,
            CancellationToken token)
        {
            IEnumerable<DeviceState> listResults = await deviceStates.FindByAsync(filter, orderBy, token);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<PaginatedList<DeviceStateConfig>> GetDeviceStatePageAsync(Guid deviceId, 
            string filter,
            int pageIndex,
            int pageSize,
            CancellationToken token)
        {
            Device device = await devices.FindByKeyAsync(deviceId, token);
            if (device == null)
            {
                logger.LogError($"Device Id {deviceId.ToString()} was not found.");
                return null;
            }

            IEnumerable<DeviceState> listResults = await deviceStates.FindByAsync(s => s.DeviceId == deviceId, token);

            return listResults
                .Where(f => string.IsNullOrEmpty(filter) || f.State.Name.Contains(filter))
                .Select(s => s.ToViewModel())
                .ToPaginatedList(pageIndex, pageSize);
        }

        public async Task<DeviceStateConfig> SaveDeviceStateAsync(DeviceStateConfig model,
            CancellationToken token)
        {
            DeviceState deviceStateItem = null;

            try
            {
                RepositoryResult result = null;

                if (!model.DeviceStateId.HasValue)
                {
                    result = await deviceStates.CreateAsync(model.ToModel(), token);
                }
                else
                {
                    result = await deviceStates.UpdateAsync(model.ToModel(), token);
                }

                if (result.Succeeded)
                {
                    logger.LogInformation($"Created device sensor {model.Name}");

                    IEnumerable<DeviceState> searchResults = await deviceStates.FindByAsync(s => s.State.Name == model.Name, token);
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
            CancellationToken token)
        {
            try
            {
                RepositoryResult result = await deviceStates.DeleteAsync(model.ToModel(), token);

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