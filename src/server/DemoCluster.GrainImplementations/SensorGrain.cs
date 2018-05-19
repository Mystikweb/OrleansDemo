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

        public Task Initialize(DeviceSensorConfig config)
        {
            State = config.ToSensorState();
            //State.Device = deviceName;

            return Task.CompletedTask;
        }

        public Task<SensorState> GetState()
        {
            return Task.FromResult(State);
        }

        public Task StartReceiving()
        {
            State.IsReceiving = true;
            return Task.CompletedTask;
        }

        public Task StopReceiving()
        {
            State.IsReceiving = false;
            return Task.CompletedTask;
        }
    }
}
