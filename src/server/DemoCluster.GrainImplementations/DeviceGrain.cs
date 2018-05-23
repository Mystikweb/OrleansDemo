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
    [LogConsistencyProvider(ProviderName="StateStorage")]
    [StorageProvider(ProviderName = "DeviceStorage")]
    public class DeviceGrain : 
        JournaledGrain<DeviceState, DeviceUpdateEvent>,
        IRemindable,
        IDeviceGrain
    {
        private readonly IConfigurationStorage configuration;
        private readonly ILogger<DeviceGrain> logger;
        private DeviceConfig config;

        public DeviceGrain(IConfigurationStorage configuration, ILoggerFactory loggerFactory)
        {
            this.configuration = configuration;
            this.logger = loggerFactory.CreateLogger<DeviceGrain>();
        }

        public override async Task OnActivateAsync()
        {
            config = await configuration.GetDeviceByIdAsync(this.GetPrimaryKey().ToString());
            await RegisterOrUpdateReminder("PushState", TimeSpan.FromSeconds(1), TimeSpan.FromMinutes(1));
        }

        public async Task ReceiveReminder(string reminderName, TickStatus status)
        {
            if (reminderName == "PushState")
            {
                await PushState(new DeviceUpdateEvent() { DeviceId = this.GetPrimaryKey(), Name = State.Name, IsRunning = State.IsRunning });
            }
        }

        public Task<DeviceState> GetCurrentState()
        {
            return Task.FromResult(State);
        }

        public async Task Start()
        {
            logger.Info($"Starting {this.GetPrimaryKey().ToString()}...");
            await PushState(new DeviceUpdateEvent() { DeviceId = this.GetPrimaryKey(), Name = config.Name, IsRunning = true });
        }

        public async Task Stop()
        {
            logger.Info($"Stopping {this.GetPrimaryKey().ToString()}...");
            await PushState(new DeviceUpdateEvent() { DeviceId = this.GetPrimaryKey(), Name = State.Name, IsRunning = false });
        }

        private async Task PushState(DeviceUpdateEvent updateEvent)
        {
            RaiseEvent(updateEvent);
            await ConfirmEvents();
        }

    }
}
