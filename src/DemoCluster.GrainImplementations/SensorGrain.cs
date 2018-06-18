using System.Threading.Tasks;
using DemoCluster.DAL;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces;
using DemoCluster.GrainInterfaces.States;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Providers;

namespace DemoCluster.GrainImplementations
{
    [StorageProvider(ProviderName="CacheStorage")]
    public class SensorGrain :
        Grain<SensorState>,
        ISensorGrain
    {
        private readonly IConfigurationStorage configuration;
        private readonly ILogger<SensorGrain> logger;

        private SensorDeviceConfig config;
        private ISensorStatusHistoryGrain statusHistory;

        public SensorGrain(IConfigurationStorage configuration, ILogger<SensorGrain> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        public override async Task OnActivateAsync()
        {
            statusHistory = GrainFactory.GetGrain<ISensorStatusHistoryGrain>(this.GetPrimaryKeyLong());

            await ReadStateAsync();
            if (State.DeviceSensorId == 0)
            {
                config = await configuration.GetDeviceSensorAsync((int)this.GetPrimaryKeyLong());
                State.DeviceSensorId = (int)this.GetPrimaryKeyLong();
                State.Name = config.SensorName;
                State.IsReceiving = await statusHistory.GetIsReceiving();
                await WriteStateAsync();
            }
        }

        public Task<bool> StartReceiving()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> StopReceiving()
        {
            throw new System.NotImplementedException();
        }
    }
}