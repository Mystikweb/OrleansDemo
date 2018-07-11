using System.Threading.Tasks;
using DemoCluster.DAL;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces;
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
            
        }

        public Task<DeviceConfig> GetCurrentConfig()
        {
            throw new System.NotImplementedException();
        }

        public Task<DeviceStateItem> GetCurrentStatus()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateConfig()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateCurrentStatus(DeviceStateItem state)
        {
            throw new System.NotImplementedException();
        }
    }
}