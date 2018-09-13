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
    public class SensorLogic
    {
        private readonly ILogger logger;
        private readonly IRepository<Sensor, ConfigurationContext> sensors;

        public SensorLogic(ILogger<SensorLogic> logger, IRepository<Sensor, ConfigurationContext> sensors)
        {
            this.logger = logger;
            this.sensors = sensors;
        }

        public async Task<List<SensorConfig>> GetSensorListAsync(CancellationToken token)
        {
            IEnumerable<Sensor> listResults = await sensors.AllAsync(token);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<List<SensorConfig>> GetSensorListAsync(Expression<Func<Sensor, bool>> filter,
            CancellationToken token)
        {
            IEnumerable<Sensor> listResults = await sensors.FindByAsync(filter, token);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<List<SensorConfig>> GetSensorListAsync(Expression<Func<Sensor, bool>> filter,
            Func<IQueryable<Sensor>, IOrderedQueryable<Sensor>> orderBy,
            CancellationToken token)
        {
            IEnumerable<Sensor> listResults = await sensors.FindByAsync(filter, orderBy, token);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<PaginatedList<SensorConfig>> GetSensorPageAsync(string filter, 
            int pageIndex, 
            int pageSize,
            CancellationToken token)
        {
            IEnumerable<Sensor> listResults = await sensors.AllAsync(token);

            return listResults
                .Where(s => string.IsNullOrEmpty(filter) || s.Name.Contains(filter))
                .Select(o => o.ToViewModel())
                .ToPaginatedList(pageIndex, pageSize);
        }

        public async Task<SensorConfig> GetSensorAsync(int sensorId,
            CancellationToken token)
        {
            Sensor result = await sensors.FindByKeyAsync(sensorId, token);

            return result?.ToViewModel();
        }

        public async Task<SensorConfig> GetSensorAsync(string sensorName,
            CancellationToken token)
        {
            Sensor result = await sensors.FindByKeyAsync(sensorName, token);

            return result?.ToViewModel();
        }

        public async Task<SensorConfig> SaveSensorAsync(SensorConfig model,
            CancellationToken token)
        {
            Sensor sensorItem = null;

            try
            {
                RepositoryResult result = null;

                if (!model.SensorId.HasValue)
                {
                    result = await sensors.CreateAsync(model.ToModel(), token);
                }
                else
                {
                    result = await sensors.UpdateAsync(model.ToModel(), token);
                }

                if (result.Succeeded)
                {
                    logger.LogInformation($"Created device {model.Name}");

                    sensorItem = await sensors.FindByKeyAsync(model.Name, token);
                    if (sensorItem == null)
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

            return sensorItem?.ToViewModel();
        }

        public async Task RemoveSensorAsync(SensorConfig model,
            CancellationToken token)
        {
            try
            {
                RepositoryResult result = await sensors.DeleteAsync(model.ToModel(), token);

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