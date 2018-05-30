using System.Collections.Generic;
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
    [LogConsistencyProvider(ProviderName = "StateStorage")]
    [StorageProvider(ProviderName="SqlBase")]
    public class DeviceInstanceGrain :
        JournaledGrain<DeviceInstanceState>,
        IDeviceInstanceGrain
    {
        private readonly IConfigurationStorage configurationStorage;
        private readonly IRuntimeStorage runtimeStorage;
        private readonly ILogger<DeviceInstanceGrain> logger;

        public DeviceInstanceGrain(IConfigurationStorage configurationStorage, IRuntimeStorage runtimeStorage, ILoggerFactory loggerFactory)
        {
            this.configurationStorage = configurationStorage;
            this.runtimeStorage = runtimeStorage;
            this.logger = loggerFactory.CreateLogger<DeviceInstanceGrain>();
        }

        public override async Task OnActivateAsync()
        {
            await RefreshNow();
        }

        public Task<DeviceInstanceState> UpdateSensors(List<SensorConfig> sensorList)
        {
            throw new System.NotImplementedException();
        }

        public Task<DeviceInstanceState> UpdateStatus(DeviceStateConfig newStatus)
        {
            throw new System.NotImplementedException();
        }
    }
}