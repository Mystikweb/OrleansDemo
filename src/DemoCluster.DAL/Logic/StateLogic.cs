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
    public class StateLogic
    {
        private readonly ILogger logger;
        private readonly IRepository<State, ConfigurationContext> states;

        public StateLogic(ILogger<StateLogic> logger, IRepository<State, ConfigurationContext> states)
        {
            this.logger = logger;
            this.states = states;
        }

        public async Task<List<StateConfig>> GetStateListAsync(CancellationToken token = default)
        {
            IEnumerable<State> listResults = await states.AllAsync(token);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<List<StateConfig>> GetStateListAsync(Expression<Func<State, bool>> filter,
            CancellationToken token = default)
        {
            IEnumerable<State> listResults = await states.FindByAsync(filter, token);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<List<StateConfig>> GetStateListAsync(Expression<Func<State, bool>> filter,
            Func<IQueryable<State>, IOrderedQueryable<State>> orderBy,
            CancellationToken token = default)
        {
            IEnumerable<State> listResults = await states.FindByAsync(filter, orderBy, token);

            return listResults
                .Select(d => d.ToViewModel())
                .ToList();
        }

        public async Task<PaginatedList<StateConfig>> GetStatePageAsync(string filter, 
            int pageIndex, 
            int pageSize,
            CancellationToken token = default)
        {
            IEnumerable<State> listResults = await states.AllAsync(token);

            return listResults
                .Where(s => string.IsNullOrEmpty(filter) || s.Name.Contains(filter))
                .Select(o => o.ToViewModel())
                .ToPaginatedList(pageIndex, pageSize);
        }

        public async Task<StateConfig> GetStateAsync(int eventId,
            CancellationToken token = default)
        {
            State result = await states.FindByKeyAsync(eventId, token);

            return result?.ToViewModel();
        }

        public async Task<StateConfig> GetStateAsync(string eventName,
            CancellationToken token = default)
        {
            State result = await states.FindByKeyAsync(eventName, token);

            return result?.ToViewModel();
        }

        public async Task<StateConfig> SaveStateAsync(StateConfig model,
            CancellationToken token = default)
        {
            State stateItem = null;

            try
            {
                RepositoryResult result = null;

                if (!model.StateId.HasValue)
                {
                    result = await states.CreateAsync(model.ToModel(), token);
                }
                else
                {
                    result = await states.UpdateAsync(model.ToModel(), token);
                }

                if (result.Succeeded)
                {
                    logger.LogInformation($"Created device {model.Name}");

                    stateItem = await states.FindByKeyAsync(model.Name, token);
                    if (stateItem == null)
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

            return stateItem?.ToViewModel();
        }

        public async Task RemoveStateAsync(StateConfig model,
            CancellationToken token = default)
        {
            try
            {
                RepositoryResult result = await states.DeleteAsync(model.ToModel(), token);

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