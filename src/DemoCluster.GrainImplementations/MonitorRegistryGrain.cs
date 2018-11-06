using System;
using System.Linq;
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
        private CancellationToken cancellation;

        public MonitorRegistryGrain(ILogger<MonitorRegistryGrain> logger)
        {
            this.logger = logger;
        }

        public Task Initialize(CancellationToken cancellation)
        {
            this.cancellation = cancellation;

            return Task.CompletedTask;
        }

        public Task ConfigureMonitor(MonitorConfig config)
        {
            return Task.CompletedTask;
        }
    }
}