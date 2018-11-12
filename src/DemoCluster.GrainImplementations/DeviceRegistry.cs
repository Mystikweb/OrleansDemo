using DemoCluster.GrainInterfaces;
using Microsoft.Extensions.Logging;
using Orleans.MultiCluster;
using Orleans.Providers;

namespace DemoCluster.GrainImplementations
{
    [OneInstancePerCluster]
    [StorageProvider(ProviderName = "CacheStorage")]
    public class DeviceRegistry : RegistryGrain<IDeviceGrain>, IDeviceRegistry
    {
        private readonly ILogger logger;

        public DeviceRegistry(ILogger<DeviceRegistry> logger)
        {
            this.logger = logger;
        }
    }
}
