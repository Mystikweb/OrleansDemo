using DemoCluster.GrainInterfaces;
using DemoCluster.GrainInterfaces.States;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.EventSourcing;
using Orleans.EventSourcing.CustomStorage;
using Orleans.Providers;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoCluster.GrainImplementations
{
    [LogConsistencyProvider(ProviderName = "CustomStorage")]
    public class SensorJournalGrain :
        JournaledGrain<SensorState, SensorHistoryState>,
        ICustomStorageInterface<SensorState, SensorHistoryState>,
        ISensorJournalGrain
    {
        private readonly ILogger logger;

        public SensorJournalGrain(ILogger<SensorJournalGrain> logger)
        {
            this.logger = logger;
        }

        public Task<SensorState> Initialize(SensorState initialState)
        {
            return Task.FromResult(initialState);
        }

        public Task PushState(SensorHistoryState state)
        {
            return Task.CompletedTask;
        }

        public Task<bool> ApplyUpdatesToStorage(IReadOnlyList<SensorHistoryState> updates, int expectedversion)
        {
            throw new NotImplementedException();
        }

        public Task<KeyValuePair<int, SensorState>> ReadStateFromStorage()
        {
            throw new NotImplementedException();
        }
    }
}
