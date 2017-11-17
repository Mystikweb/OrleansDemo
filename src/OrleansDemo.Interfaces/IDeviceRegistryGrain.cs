using OrleansDemo.Patterns.Registry;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrleansDemo.Interfaces
{
    public interface IDeviceRegistryGrain : IRegistryGrain<IDeviceGrain>
    {
        Task<bool> HasDevice(Guid deviceId);
    }
}
