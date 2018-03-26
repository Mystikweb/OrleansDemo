using DemoCluster.GrainInterfaces;
using DemoCluster.GrainInterfaces.States;
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
        private Logger logger;

        public override Task OnActivateAsync()
        {
            logger = GetLogger($"SensorJournal_{this.GetPrimaryKeyLong()}");

            return base.OnActivateAsync();
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
