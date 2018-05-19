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
using System.Linq;
using System.Threading.Tasks;

namespace DemoCluster.GrainImplementations
{
    [LogConsistencyProvider(ProviderName = "CustomStorage")]
    public class DeviceJournalGrain :
        JournaledGrain<DeviceState, DeviceHistoryState>,
        ICustomStorageInterface<DeviceState, DeviceHistoryState>,
        IDeviceJournalGrain
    {
        private readonly IRuntimeStorage storage;
        private readonly ILogger logger;

        private DeviceState internalState;

        public DeviceJournalGrain(IRuntimeStorage storage, ILogger<DeviceJournalGrain> logger)
        {
            this.storage = storage;
            this.logger = logger;
        }

        protected override void TransitionState(DeviceState state, DeviceHistoryState delta)
        {
            // state.Apply(delta);
        }

        public async Task<DeviceState> Initialize(DeviceConfig config)
        {
            internalState = config.ToDeviceGrainState();

            await RefreshNow();

            return State;
        }

        public async Task PushState(DeviceHistoryState historyState)
        {
            logger.Info($"Adding history for {State.Name} at {historyState.Timestamp.ToString()}");
            RaiseEvent(historyState);

            await ConfirmEvents();
        }

        public Task<KeyValuePair<int, DeviceState>> ReadStateFromStorage()
        {
            // if (internalState != null)
            // {
            //     var historyItems = await storage.GetDeviceStateHistory(this.GetPrimaryKey());

            //     foreach (var item in historyItems.OrderBy(i => i.Timestamp))
            //     {
            //         internalState.Apply(item.ToDeviceHistoryState());
            //     }

            //     int version = internalState.History.Count;

            //     return new KeyValuePair<int, DeviceState>(version, internalState);
            // }

            return Task.FromResult(new KeyValuePair<int, DeviceState>(0, new DeviceState()));
        }

        public async Task<bool> ApplyUpdatesToStorage(IReadOnlyList<DeviceHistoryState> updates, int expectedversion)
        {
            try
            {
                foreach (var item in updates)
                {
                    await storage.SaveDeviceState(item.ToDeviceHistoryItem());
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(5001, $"Error storing updates.", ex);
                return false;
            }
        }
    }
}
