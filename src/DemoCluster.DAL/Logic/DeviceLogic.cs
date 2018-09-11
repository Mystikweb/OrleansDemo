using System;
using System.Collections.Generic;
using System.Linq;
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

        public DeviceLogic(ILogger<DeviceLogic> logger, IRepository<Device, ConfigurationContext> devices)
        {
            this.logger = logger;
            this.devices = devices;
        }

        public async Task<PaginatedList<DeviceConfig>> GetListAsync(string filter, int pageIndex, int pageSize, CancellationToken token)
        {
            IEnumerable<Device> listResults = await devices.AllAsync(token);

            return listResults
                .Where(s => string.IsNullOrEmpty(filter) || s.Name.Contains(filter))
                .Select(o => o.ToViewModel())
                .ToPaginatedList(pageIndex, pageSize);
        }

        public async Task<DeviceConfig> GetById(Guid deviceId, CancellationToken token)
        {
            Device result = await devices.FindByKeyAsync(deviceId, token);

            return result?.ToViewModel();
        }

        public async Task<DeviceConfig> GetByName(string deviceName, CancellationToken token)
        {
            Device result = await devices.FindByKeyAsync(deviceName, token);

            return result?.ToViewModel();
        }

        public async Task<DeviceConfig> SaveAsync(DeviceConfig model, CancellationToken token)
        {
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
                    logger.LogInformation($"Created service {model.Name}");

                    Device deviceItem = await devices.FindByKeyAsync(model.Name, token);
                    if (deviceItem != null)
                    {
                        return deviceItem.ToViewModel();
                    }
                    else
                    {
                        logger.LogError($"Unable to find service {model.Name} as result.");
                    }
                }
                else
                {
                    LogErrors(result.Errors);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error creating service {model.Name}");
                throw;
            }

            return null;
        }

        public async Task RemoveAsync(DeviceConfig model, CancellationToken token)
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

        private void LogErrors(IEnumerable<RepositoryError> errors)
        {
            foreach (var error in errors)
            {
                logger.LogError($"{error.Code} - {error.Description}");
            }
        }
    }
}