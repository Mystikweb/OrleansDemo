using Orleans;
using Orleans.Providers;
using OrleansDemo.Interfaces;
using OrleansDemo.Interfaces.State;
using OrleansDemo.Models.Transfer;
using OrleansDemo.Models.ViewModels.Runtime;
using OrleansDemo.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OrleansDemo.Implementations
{
    [StorageProvider(ProviderName = "OrleansDemoStorage")]
    public class DeviceGrain : Grain<DeviceGrainState>, IDeviceGrain
    {
        private readonly IRuntimeReading readings;
        private DeviceConfiguration config;

        public DeviceGrain(IRuntimeReading runtimeReading)
        {
            readings = runtimeReading;
        }

        public Task<Guid> GetDeviceId()
        {
            return Task.FromResult(State.Id);
        }

        public Task Initialize(DeviceConfiguration configuration)
        {
            State.Id = configuration.DeviceId;
            State.Name = configuration.Name;
            State.IsRunning = false;

            config = configuration;

            return Task.CompletedTask;
        }

        public Task Start()
        {
            State.IsRunning = true;
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            State.IsRunning = false;
            return Task.CompletedTask;
        }

        public override Task OnActivateAsync()
        {
            

            return base.OnActivateAsync();
        }

        public async Task RecordValue(string value)
        {
            var readingType = config.ReadingConfigurations.FirstOrDefault();

            ReadingViewModel model = new ReadingViewModel
            {
                Device = State.Name,
                Type = config.DeviceType,
                DataType = readingType.DataType,
                UOM = readingType.UOM,
                Value = value
            };

            await readings.SaveReading(model);
        }
    }
}
