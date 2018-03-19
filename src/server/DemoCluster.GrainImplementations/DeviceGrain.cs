using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoCluster.DAL;
using DemoCluster.DAL.States;
using DemoCluster.GrainInterfaces;
using Orleans;
using Orleans.EventSourcing;
using Orleans.EventSourcing.CustomStorage;

namespace DemoCluster.GrainImplementations
{
    public class DeviceGrain :
        JournaledGrain<DeviceHistory, DeviceHistoryState>,
        ICustomStorageInterface<DeviceHistory, DeviceHistoryState>,
        IDeviceGrain
    {
        private readonly IRuntimeStorage storage;

        public DeviceGrain(IRuntimeStorage storage)
        {
            this.storage = storage;
        }

        public override Task OnActivateAsync()
        {
            return base.OnActivateAsync();
        }

        public Task Start()
        {
            return Task.CompletedTask;
        }

        public async Task<KeyValuePair<int, DeviceHistory>> ReadStateFromStorage()
        {
            var historyItems = await storage.GetDeviceHistory(this.GetPrimaryKey());

            DeviceHistory state = new DeviceHistory();

            foreach (var item in historyItems.OrderBy(i => i.TimeStamp))
            {
                state.Apply(item.ToState());
            }

            int version = state.StateHistory.Count;

            return new KeyValuePair<int, DeviceHistory>(version, state);
        }

        public Task<bool> ApplyUpdatesToStorage(IReadOnlyList<DeviceHistoryState> updates, int expectedversion)
        {
            return Task.FromResult(true);
        }
    }
}
