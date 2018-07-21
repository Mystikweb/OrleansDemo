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
        private readonly IConfigurationStorage storage;
        private CancellationToken cancellation;

        public MonitorRegistryGrain(ILogger<MonitorRegistryGrain> logger, IConfigurationStorage storage)
        {
            this.logger = logger;
            this.storage = storage;
        }

        public async Task Initialize(CancellationToken cancellation)
        {
            this.cancellation = cancellation;

            var monitors = await storage.GetMonitorListAsync();
            foreach (var monitor in monitors.Where(m => m.IsEnabled && m.RunAtStartup))
            {
                var monitorGrain = GrainFactory.GetGrain<IMessageMonitorGrain>(Guid.Parse(monitor.MonitorId));

                bool isSetup = await monitorGrain.UpdateMonitor(monitor);
                await RegisterGrain(monitorGrain);

                if (!isSetup)
                {
                    logger.LogWarning($"Monitor with id {monitor.MonitorId} was not initialized successfully.");
                }
            }
        }

        public Task ConfigureMonitor(MonitorConfig config)
        {
            return Task.CompletedTask;
        }
    }
}