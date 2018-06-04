using DemoCluster.DAL;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces;
using DemoCluster.GrainInterfaces.Commands;
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
            statusHistory = GrainFactory.GetGrain<IDeviceStatusHistoryGrain>(this.GetPrimaryKey());

            await ReadStateAsync();
            if (State.DeviceId == Guid.Empty)
            {
                config = await configuration.GetDeviceByIdAsync(this.GetPrimaryKey().ToString());
                State = config.ToDeviceGrainState();
                State.CurrentState = await statusHistory.GetCurrentStatus();
                await WriteStateAsync();
            }
        }

        public async Task Start()
        {
            logger.Info($"Starting {this.GetPrimaryKey().ToString()}...");
            if (string.IsNullOrEmpty(State.CurrentState.Name) || State.CurrentState.Name.ToUpper() != "RUNNING")
            {
                var configState = config.States.Where(s => s.Name.ToUpper() == "RUNNING").FirstOrDefault();
                await statusHistory.UpdateStatus(new DeviceStatusCommand(configState.DeviceStateId.Value,
                    configState.Name));
                State.CurrentState = await statusHistory.GetCurrentStatus();
                await WriteStateAsync();
            }
        }

        public async Task Stop()
        {
            logger.Info($"Stopping {this.GetPrimaryKey().ToString()}...");
            if (string.IsNullOrEmpty(State.CurrentState.Name) || State.CurrentState.Name.ToUpper() != "STOPPED")
            {
                var configState = config.States.Where(s => s.Name.ToUpper() == "STOPPED").FirstOrDefault();
                await statusHistory.UpdateStatus(new DeviceStatusCommand(configState.DeviceStateId.Value,
                    configState.Name));
                State.CurrentState = await statusHistory.GetCurrentStatus();
                await WriteStateAsync();
            }
        }
    }
}
