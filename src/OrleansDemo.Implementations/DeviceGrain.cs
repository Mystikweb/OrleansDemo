using Orleans;
using Orleans.Providers;
using OrleansDemo.Interfaces;
using OrleansDemo.Interfaces.State;
using System;
using System.Threading.Tasks;

namespace OrleansDemo.Implementations
{
    [StorageProvider(ProviderName = "OrleansDemoStorage")]
    public class DeviceGrain : Grain<DeviceGrainState>, IDeviceGrain
    {
        public Task Start()
        {
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            return Task.CompletedTask;
        }
    }
}
