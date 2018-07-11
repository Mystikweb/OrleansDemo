using System;
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
using Orleans.Providers;

namespace DemoCluster.GrainImplementations
{
    [LogConsistencyProvider(ProviderName="LogStorage")]
    [StorageProvider(ProviderName = "SqlBase")]
    public class DeviceGrain : 
        JournaledGrain<DeviceState>, IDeviceGrain
    {
        private readonly ILogger logger;
        private DeviceConfig deviceConfig;

        public DeviceGrain(ILogger<DeviceGrain> logger)
        {
            this.logger = logger;
        }

        public override async Task OnActivateAsync()
        {
            await RefreshNow();
        }

        public Task<Guid> GetKey()
        {
            return Task.FromResult(this.GetPrimaryKey());
        }

        public Task<DeviceState> GetState()
        {
            return Task.FromResult(State);
        }

        public Task<DeviceConfig> GetCurrentConfig()
        {
            return Task.FromResult(deviceConfig);
        }

        public Task<DeviceStateItem> GetCurrentStatus()
        {
            return Task.FromResult(State.CurrentState.ToStateItem(this.GetPrimaryKey()));
        }

        public async Task<bool> UpdateConfig(DeviceConfig config)
        {
            deviceConfig = config;

            RaiseEvent(new DeviceConfigCommand(this.GetPrimaryKey(), deviceConfig.Name));
            await ConfirmEvents();

            await ProcessConfigStatus();

            return true;
        }

        public async Task<bool> UpdateCurrentStatus(DeviceStateItem state)
        {
            RaiseEvent(new DeviceStatusCommand(state.DeviceStatusId, state.StatusId, state.Name));
            await ConfirmEvents();

            return true;
        }

        private async Task ProcessConfigStatus()
        {
            if (deviceConfig.IsEnabled && State.CurrentState.Name != "RUNNING")
            {
                var runningState = deviceConfig.States.FirstOrDefault(s => s.IsEnabled && s.Name == "RUNNING");
                if (runningState != null)
                {
                    await UpdateCurrentStatus(runningState.ConfigToStateItem(this.GetPrimaryKey()));
                }
            }

            if (!deviceConfig.IsEnabled && State.CurrentState.Name != "STOPPED")
            {
                var stoppedState = deviceConfig.States.FirstOrDefault(s => s.IsEnabled && s.Name == "STOPPED");
                if (stoppedState != null)
                {
                    await UpdateCurrentStatus(stoppedState.ConfigToStateItem(this.GetPrimaryKey()));
                }
            }
        }
    }
}