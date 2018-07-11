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
        private readonly IConfigurationStorage configuration;

        private DeviceConfig deviceConfig;

        public DeviceGrain(ILogger<DeviceGrain> logger, IConfigurationStorage configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public override async Task OnActivateAsync()
        {
            deviceConfig = await configuration.GetDeviceByIdAsync(this.GetPrimaryKey().ToString());
            await RefreshNow();
        }

        public Task<DeviceConfig> GetCurrentConfig()
        {
            return Task.FromResult(deviceConfig);
        }

        public Task<DeviceStateItem> GetCurrentStatus()
        {
            return Task.FromResult(State.CurrentState.ToStateItem(this.GetPrimaryKey()));
        }

        public Task<bool> UpdateConfig()
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> UpdateCurrentStatus(DeviceStateItem state)
        {
            RaiseEvent(new DeviceStatusCommand(state.DeviceStatusId, state.StatusId, state.Name));
            await ConfirmEvents();

            return true;
        }
    }
}