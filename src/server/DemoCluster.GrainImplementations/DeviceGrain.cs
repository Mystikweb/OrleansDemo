using DemoCluster.DAL;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces;
using DemoCluster.GrainInterfaces.States;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.EventSourcing;
using Orleans.Providers;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoCluster.GrainImplementations
{
    [StorageProvider(ProviderName="CacheStorage")]
    public class DeviceGrain : 
        Grain<DeviceState>,
        IDeviceGrain
    {
        private readonly IConfigurationStorage configuration;
        private readonly ILogger<DeviceGrain> logger;

        private DeviceConfig config;
        private IDeviceStatusHistoryGrain statusHistory;

        public DeviceGrain(IConfigurationStorage configuration, ILoggerFactory loggerFactory)
        {
            this.configuration = configuration;
            this.logger = loggerFactory.CreateLogger<DeviceGrain>();
        }

        public override async Task OnActivateAsync()
        {
            config = await configuration.GetDeviceByIdAsync(this.GetPrimaryKey().ToString());
            statusHistory = GrainFactory.GetGrain<IDeviceStatusHistoryGrain>(this.GetPrimaryKey());
            State = config.ToDeviceGrainState();
        }

        public Task Start()
        {
            logger.Info($"Starting {this.GetPrimaryKey().ToString()}...");
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            logger.Info($"Stopping {this.GetPrimaryKey().ToString()}...");
            return Task.CompletedTask;
        }
    }
}
