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
    public class EventLogic
    {
        private readonly ILogger logger;
        private readonly IRepository<EventType, ConfigurationContext> events;

        public EventLogic(ILogger<EventLogic> logger, IRepository<EventType, ConfigurationContext> events)
        {
            this.logger = logger;
            this.events = events;
        }

        public async Task<List<EventConfig>> GetEventListAsync(CancellationToken token)
        {
            IEnumerable<EventType> listResults = await events.AllAsync(token);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<List<EventConfig>> GetEventListAsync(Expression<Func<EventType, bool>> filter,
            CancellationToken token)
        {
            IEnumerable<EventType> listResults = await events.FindByAsync(filter, token);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<List<EventConfig>> GetEventListAsync(Expression<Func<EventType, bool>> filter,
            Func<IQueryable<EventType>, IOrderedQueryable<EventType>> orderBy,
            CancellationToken token)
        {
            IEnumerable<EventType> listResults = await events.FindByAsync(filter, orderBy, token);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<PaginatedList<EventConfig>> GetEventPageAsync(string filter, 
            int pageIndex, 
            int pageSize,
            CancellationToken token)
        {
            IEnumerable<EventType> listResults = await events.AllAsync(token);

            return listResults
                .Where(s => string.IsNullOrEmpty(filter) || s.Name.Contains(filter))
                .Select(o => o.ToViewModel())
                .ToPaginatedList(pageIndex, pageSize);
        }

        public async Task<EventConfig> GetEventAsync(int eventId,
            CancellationToken token)
        {
            EventType result = await events.FindByKeyAsync(eventId, token);

            return result?.ToViewModel();
        }

        public async Task<EventConfig> GetEventAsync(string eventName,
            CancellationToken token)
        {
            EventType result = await events.FindByKeyAsync(eventName, token);

            return result?.ToViewModel();
        }

        public async Task<EventConfig> SaveEventAsync(EventConfig model,
            CancellationToken token)
        {
            EventType eventItem = null;

            try
            {
                RepositoryResult result = null;

                if (!model.EventId.HasValue)
                {
                    result = await events.CreateAsync(model.ToModel(), token);
                }
                else
                {
                    result = await events.UpdateAsync(model.ToModel(), token);
                }

                if (result.Succeeded)
                {
                    logger.LogInformation($"Created device {model.Name}");

                    eventItem = await events.FindByKeyAsync(model.Name, token);
                    if (eventItem == null)
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

            return eventItem?.ToViewModel();
        }

        public async Task RemoveEventAsync(EventConfig model,
            CancellationToken token)
        {
            try
            {
                RepositoryResult result = await events.DeleteAsync(model.ToModel(), token);

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