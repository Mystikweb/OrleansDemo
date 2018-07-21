using System.Threading;
using System.Threading.Tasks;
using DemoCluster.GrainInterfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;

namespace DemoCluster.GrainImplementations
{
    public class MessageMonitorStartup : IStartupTask
    {
        private readonly IGrainFactory factory;
        private readonly ILogger logger;

        public MessageMonitorStartup(IGrainFactory factory, ILogger<MessageMonitorStartup> logger)
        {
            this.factory = factory;
            this.logger = logger;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            var registry = factory.GetGrain<IMonitorRegistryGrain>(0);

            logger.LogInformation($"Starting monitor registry...");
            await registry.Initialize(cancellationToken);
        }
    }
}