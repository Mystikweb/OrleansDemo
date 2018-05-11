using DemoCluster.DAL;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces;
using DemoCluster.GrainInterfaces.States;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Providers;
using System.Threading.Tasks;

namespace DemoCluster.GrainImplementations
{
    [StorageProvider(ProviderName = "MemoryStorage")]
    public class SensorGrain : Grain<SensorState>, ISensorGrain
    {
        private readonly IRuntimeStorage storage;
        private readonly ILogger logger;

        public SensorGrain(IRuntimeStorage storage, ILogger<SensorGrain> logger)
        {
            this.storage = storage;
            this.logger = logger;
        }

        public override Task OnActivateAsync()
        {

            return base.OnActivateAsync();
        }

        public Task Initialize(DeviceSensorConfig config, string deviceName)
        {
            State = config.ToSensorState();

            return Task.CompletedTask;
        }

        public Task<bool> GetIsReceiving()
        {
            return Task.FromResult(true);
        }

        public Task StartReceiving()
        {
            return Task.CompletedTask;
        }

        public Task StopReceiving()
        {
            return Task.CompletedTask;
        }
    }
}
