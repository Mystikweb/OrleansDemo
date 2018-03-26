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
        private IDeviceJournalGrain journalGrain;

        public DeviceGrain(IRuntimeStorage storage)
        {
            this.storage = storage;
        }

        public override Task OnActivateAsync()
        {
            logger = GetLogger($"Device_{this.GetPrimaryKey().ToString()}");
            journalGrain = GrainFactory.GetGrain<IDeviceJournalGrain>(this.GetPrimaryKey());

            return base.OnActivateAsync();
        }

        public Task<bool> GetIsRunning()
        {
            return Task.FromResult(State.IsRunning);
        }

        public async Task Start(DeviceConfig config)
        {
            logger.Info($"Starting {this.GetPrimaryKey().ToString()}...");

            State = await journalGrain.Initialize(config.ToState());
            State.IsRunning = true;

            foreach (var sensor in config.Sensors.Where(s => s.IsEnabled))
            {
                var sensorGrain = GrainFactory.GetGrain<ISensorGrain>(sensor.DeviceSensorId.Value);
                State.RegisteredSensors.Add(sensorGrain);
            }

            await WriteStateAsync();
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
            await journalGrain.PushState(new DeviceHistoryState
            {
                DeviceId = State.DeviceId,
                IsRunning = State.IsRunning,
                SensorCount = State.RegisteredSensors.Count
            });
        }
    }
}
