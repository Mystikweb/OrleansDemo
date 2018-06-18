using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoCluster.DAL;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces;
using DemoCluster.GrainInterfaces.Commands;
using DemoCluster.GrainInterfaces.States;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.EventSourcing;
using Orleans.EventSourcing.CustomStorage;
using Orleans.Providers;

namespace DemoCluster.GrainImplementations
{
    [LogConsistencyProvider(ProviderName="LogStorage")]
    public class SensorStatusHistoryGrain :
        JournaledGrain<SensorStatusState, SensorStatusCommand>,
        ICustomStorageInterface<SensorStatusState, SensorStatusCommand>,
        ISensorStatusHistoryGrain
    {
        private readonly IConfigurationStorage configuration;
        private readonly IRuntimeStorage runtime;
        private readonly ILogger logger;

        private SensorDeviceConfig config;

        public SensorStatusHistoryGrain(IConfigurationStorage configuration, IRuntimeStorage runtime, ILogger<SensorStatusHistoryGrain> logger)
        {
            this.configuration = configuration;
            this.runtime = runtime;
            this.logger = logger;
        }

        public override async Task OnActivateAsync() 
        {
            config = await configuration.GetDeviceSensorAsync((int)this.GetPrimaryKeyLong());
            State.DeviceSensorId = config.DeviceSensorId;
            State.DeviceName = config.DeviceName;
            State.SensorName = config.SensorName;
            State.Uom = config.Uom;

            await RefreshNow();
        }

        public Task<bool> GetIsReceiving()
        {
            return Task.FromResult(State.IsReceiving);
        }

        public Task<List<SensorStatusHistory>> GetStatusHistory()
        {
            return Task.FromResult(State.History.Select(h => h.Value).ToList());
        }

        public async Task<bool> UpdateStatus(SensorStatusCommand update)
        {
            RaiseEvent(update);
            await ConfirmEvents();

            return true;
        }

        public async Task<KeyValuePair<int, SensorStatusState>> ReadStateFromStorage()
        {
            var historyItems = await runtime.GetSensorStateHistory((int)this.GetPrimaryKeyLong());

            int version = 0;
            foreach (var item in historyItems)
            {
                State.Apply(item.CreateSensorStatusCommand());
                version = item.Version;
            }

            return new KeyValuePair<int, SensorStatusState>(version, State);
        }

        public async Task<bool> ApplyUpdatesToStorage(IReadOnlyList<SensorStatusCommand> updates, int expectedversion)
        {
            int version = 0;
            try
            {
                foreach (var item in updates)
                {
                    await runtime.SaveSensorState(item.CreateSensorStateHistory(State));
                    version = item.Version.Value;
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error applying updates to storage");
                return false;
            }

            return version == expectedversion;
        }
    }
}