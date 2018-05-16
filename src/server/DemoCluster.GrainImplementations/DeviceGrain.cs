using DemoCluster.DAL;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces;
using DemoCluster.GrainInterfaces.States;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;
using System.Linq;
using System.Threading.Tasks;

namespace DemoCluster.GrainImplementations
{
    [StorageProvider(ProviderName = "MemoryStorage")]
    public class DeviceGrain : Grain<DeviceState>, IDeviceGrain
    {
        private readonly IConfigurationStorage configuration;
        private readonly IRuntimeStorage runtime;
        private readonly ILogger logger;
        private IDeviceJournalGrain journalGrain;

        public DeviceGrain(IRuntimeStorage runtime, IConfigurationStorage configuration, ILogger<DeviceGrain> logger)
        {
            this.runtime = runtime;
            this.configuration = configuration;
            this.logger = logger;
        }

        public async override Task OnActivateAsync()
        {
            var deviceConfig = await configuration.GetDeviceByIdAsync(this.GetPrimaryKeyString());
            journalGrain = GrainFactory.GetGrain<IDeviceJournalGrain>(this.GetPrimaryKey());
        }

        public Task<bool> GetIsRunning()
        {
            return Task.FromResult(State.IsRunning);
        }

        public async Task Start(DeviceConfig config)
        {
            logger.Info($"Starting {this.GetPrimaryKey().ToString()}...");
            
            State = await journalGrain.Initialize(config);

            foreach (var sensor in config.Sensors.Where(s => s.IsEnabled))
            {
                var sensorGrain = GrainFactory.GetGrain<ISensorGrain>(sensor.DeviceSensorId.Value);
                State.RegisteredSensors.Add(sensorGrain);

                await sensorGrain.Initialize(sensor, State.Name);
                await sensorGrain.StartReceiving();
            }

            State.IsRunning = true;
            await LogState();
        }

        public async Task Stop()
        {
            logger.Info($"Stopping {this.GetPrimaryKey().ToString()}...");

            foreach (var sensor in State.RegisteredSensors)
            {
                if (await sensor.GetIsReceiving())
                {
                    await sensor.StopReceiving();
                }
            }

            State.IsRunning = false;
            await LogState();
        }

        private async Task LogState()
        {
            DeviceHistoryState currentState = new DeviceHistoryState
            {
                DeviceId = State.DeviceId,
                Name = State.Name,
                IsRunning = State.IsRunning
            };

            foreach (var sensor in State.RegisteredSensors)
            {
                var sensorStatus = await sensor.GetCurrentStatus();

                currentState.SensorStatus.Add(new SensorStatusState
                {
                    Id = sensorStatus.DeviceSensorId,
                    Name = sensorStatus.Name,
                    UOM = sensorStatus.UOM,
                    IsReceiving = sensorStatus.IsReceiving
                });   
            }

            await journalGrain.PushState(currentState);
        }
    }
}
