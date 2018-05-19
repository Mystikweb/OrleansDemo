using DemoCluster.DAL;
using DemoCluster.DAL.Models;
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
        private readonly IRuntimeStorage storage;
        private readonly ILogger logger;

        private SensorState internalState;

        public SensorJournalGrain(IRuntimeStorage storage, ILogger<SensorJournalGrain> logger)
        {
            this.storage = storage;
            this.logger = logger;
        }

        protected override void TransitionState(SensorState state, SensorHistoryState delta)
        {
            //state.Apply(delta);
        }

        public async Task<SensorState> Initialize(DeviceSensorConfig config)
        {
            internalState = config.ToSensorState();

            await RefreshNow();

            return internalState;
        }

        public async Task PushState(SensorHistoryState state)
        {
            logger.Info($"Adding history for {State.Name} at {state.Timestamp.ToString()}");
            RaiseEvent(state);

            await ConfirmEvents();
        }

        public Task<KeyValuePair<int, SensorState>> ReadStateFromStorage()
        {
            if (internalState != null)
            {
                return Task.FromResult(new KeyValuePair<int, SensorState>(0, new SensorState()));
            }

            return Task.FromResult(new KeyValuePair<int, SensorState>(0, new SensorState()));
        }

        public Task<bool> ApplyUpdatesToStorage(IReadOnlyList<SensorHistoryState> updates, int expectedversion)
        {
            return Task.FromResult(true);
        }
    }
}
