using System.Threading;
using System.Threading.Tasks;
using DemoCluster.DAL;
using DemoCluster.DAL.Models;
using DemoCluster.GrainImplementations.Patterms;
using DemoCluster.GrainInterfaces;
using Microsoft.Extensions.Logging;
using Orleans.MultiCluster;
using Orleans.Providers;

namespace DemoCluster.GrainImplementations
{
    [OneInstancePerCluster]
    [StorageProvider(ProviderName = "CacheStorage")]
    public class MonitorRegistryGrain :
        RegistryGrain<IMessageMonitorGrain>, IMonitorRegistryGrain
    {
        private readonly ILogger logger;
        private readonly IConfigurationStorage storage;
        private CancellationToken cancellation;

        public MonitorRegistryGrain(ILogger<MonitorRegistryGrain> logger, IConfigurationStorage storage)
        {
            this.logger = logger;
            this.storage = storage;
        }

        public Task Initialize(CancellationToken cancellation)
        {
            this.cancellation = cancellation;

            //GrainFactory.GetGrain

            return Task.CompletedTask;
        }

        public Task ConfigureMonitor(MonitorConfig config)
        {
            throw new System.NotImplementedException();
        }
    }

}