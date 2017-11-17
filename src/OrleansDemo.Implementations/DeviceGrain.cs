using Orleans;
using Orleans.Providers;
using OrleansDemo.Interfaces;
using OrleansDemo.Interfaces.State;
using System;
using System.Threading.Tasks;
using OrleansDemo.Models.Transfer;

namespace OrleansDemo.Implementations
{
    [StorageProvider(ProviderName = "OrleansDemoStorage")]
    public class DeviceGrain : Grain<DeviceGrainState>, IDeviceGrain
    {
        public Task<Guid> GetDeviceId()
        {
            return Task.FromResult(State.Id);
        }

        public Task Initialize(DeviceConfiguration configuration)
        {
            State.Id = configuration.DeviceId;
            State.Name = configuration.Name;
            State.IsRunning = false;

            return Task.CompletedTask;
        }

        public Task Start()
        {
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            return Task.CompletedTask;
        }

        public override Task OnActivateAsync()
        {
            return base.OnActivateAsync();
        }
    }
}
