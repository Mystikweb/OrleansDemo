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
    [LogConsistencyProvider(ProviderName = "CustomStorage")]
    public class DeviceInstanceGrain :
        JournaledGrain<DeviceInstanceState, DeviceCommand>,
        ICustomStorageInterface<DeviceInstanceState, DeviceCommand>,
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

        public Task<bool> ApplyUpdatesToStorage(IReadOnlyList<DeviceCommand> updates, int expectedversion)
        {
            try
            {
                foreach (var item in updates)
                {
                    
                }
            }
            catch (System.Exception)
            {
                
                throw;
            }

            return Task.FromResult(true);
        }

        public async Task<KeyValuePair<int, DeviceInstanceState>> ReadStateFromStorage()
        {
            var historyList = await runtimeStorage.GetDeviceStateHistory(this.GetPrimaryKey());
            foreach (var item in historyList)
            {
                State.Apply(item.ToDeviceStateCommand());
            }

            return new KeyValuePair<int, DeviceInstanceState>(historyList.Count, State);
        }
    }
}