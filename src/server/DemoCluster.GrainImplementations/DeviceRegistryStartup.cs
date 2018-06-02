using DemoCluster.GrainInterfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using System.Threading;
using System.Threading.Tasks;

namespace DemoCluster.GrainImplementations
{
    public class DeviceRegistryStartup : IStartupTask
    {
        private readonly ILogger logger;
        private readonly IGrainFactory grainFactory;
        private IDeviceRegistry registry;

        public DeviceRegistryStartup(ILogger<DeviceRegistryStartup> logger, IGrainFactory grainFactory)
        {
            this.logger = logger;
            this.grainFactory = grainFactory;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            registry = grainFactory.GetGrain<IDeviceRegistry>(0);

            logger.Info("Initializing the device registry");
            await registry.Initialize();
        }
    }
}
