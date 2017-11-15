using OrleansDemo.Interfaces;
using System;
using System.Threading.Tasks;

namespace OrleansDemo.Implementations
{
    public class DeviceGrain : IDeviceGrain
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
