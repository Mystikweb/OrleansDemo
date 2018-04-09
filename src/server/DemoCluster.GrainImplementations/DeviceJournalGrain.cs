using DemoCluster.DAL;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces;
using DemoCluster.GrainInterfaces.States;
using Orleans;
using Orleans.EventSourcing;
using Orleans.EventSourcing.CustomStorage;
using Orleans.Providers;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private Logger logger;
        private DeviceState internalState;

        public DeviceJournalGrain(IRuntimeStorage storage)
        {
            this.storage = storage;
        }

        public override Task OnActivateAsync()
        {
            logger = GetLogger($"DeviceJournal_{this.GetPrimaryKey().ToString()}");

            return Task.CompletedTask;
        }

        protected override void TransitionState(DeviceState state, DeviceHistoryState delta)
        {
            state.Apply(delta);
        }

        public async Task<DeviceState> Initialize(DeviceConfig config)
        {
            internalState = config.ToState();

            await RefreshNow();

            return State;
        }

        public async Task PushState(DeviceHistoryState historyState)
        {
            logger.Info($"Adding history for {State.Name} at {historyState.Timestamp.ToString()}");
            RaiseEvent(historyState);

            await ConfirmEvents();
        }

        public async Task<KeyValuePair<int, DeviceState>> ReadStateFromStorage()
        {
            var historyItems = await storage.GetDeviceHistory(this.GetPrimaryKey());

            foreach (var item in historyItems.OrderBy(i => i.TimeStamp))
            {
                internalState.Apply(item.ToState());
            }

            int version = internalState.History.Count;

            return new KeyValuePair<int, DeviceState>(version, internalState);
        }

        public async Task<bool> ApplyUpdatesToStorage(IReadOnlyList<DeviceHistoryState> updates, int expectedversion)
        {
            try
            {
                foreach (var item in updates)
                {
                    await storage.StoreDeviceHistory(item.ToItem(State.Name));
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
