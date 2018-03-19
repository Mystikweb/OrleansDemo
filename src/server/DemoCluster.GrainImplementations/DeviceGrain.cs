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
using Orleans.Runtime;

namespace DemoCluster.GrainImplementations
{
    public class DeviceGrain :
        JournaledGrain<DeviceHistory, DeviceHistoryState>,
        ICustomStorageInterface<DeviceHistory, DeviceHistoryState>,
        IDeviceGrain
    {
        private readonly IRuntimeStorage storage;
        private Logger logger;

        public DeviceGrain(IRuntimeStorage storage)
        {
            this.storage = storage;
        }

        public override Task OnActivateAsync()
        {
            logger = GetLogger($"Device_{this.GetPrimaryKey().ToString()}");

            return base.OnActivateAsync();
        }

        public Task Start()
        {
            logger.Info($"Starting {this.GetPrimaryKey().ToString()}...");

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
