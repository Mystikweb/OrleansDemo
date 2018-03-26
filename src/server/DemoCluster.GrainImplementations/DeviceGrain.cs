using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoCluster.DAL;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces;
using DemoCluster.GrainInterfaces.States;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;

namespace DemoCluster.GrainImplementations
{
    [StorageProvider(ProviderName="MemoryStorage")]
    public class DeviceGrain : Grain<DeviceState>, IDeviceGrain
    {
        private readonly IRuntimeStorage storage;

        private Logger logger;
        private IDeviceHistoryJournal journal;

        public DeviceGrain(IRuntimeStorage storage)
        {
            this.storage = storage;
        }

        public override async Task OnActivateAsync()
        {
            logger = GetLogger($"Device_{this.GetPrimaryKey().ToString()}");
            journal = GrainFactory.GetGrain<IDeviceHistoryJournal>(this.GetPrimaryKey());

            State = await journal.Initialize();
        }

        public Task<bool> GetIsRunning()
        {
            return Task.FromResult(State.IsRunning);
        }

        public async Task Start()
        {
            logger.Info($"Starting {this.GetPrimaryKey().ToString()}...");
            State.IsRunning = true;

            foreach (var sensor in State.Sensors)
            {

            }

            await LogState();
        }

        public async Task Stop()
        {
            logger.Info($"Stopping {this.GetPrimaryKey().ToString()}...");
            State.IsRunning = false;

            await LogState();
        }

        private async Task LogState()
        {
            await journal.PushState(new DeviceHistoryState
            {
                DeviceId = State.DeviceId,
                IsRunning = State.IsRunning
            });
        }
    }
}
