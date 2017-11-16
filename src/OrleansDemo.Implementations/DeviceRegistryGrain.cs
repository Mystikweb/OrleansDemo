using Orleans.Providers;
using OrleansDemo.Interfaces;
using OrleansDemo.Patterns.Registry;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrleansDemo.Implementations
{
    [StorageProvider(ProviderName = "OrleansDemoStorage")]
    public class DeviceRegistryGrain : RegistryGrain<IDeviceGrain>, IDeviceRegistryGrain
    {
    }
}
