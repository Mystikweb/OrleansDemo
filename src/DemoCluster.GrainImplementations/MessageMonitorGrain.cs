using System.Threading.Tasks;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces;
using DemoCluster.GrainInterfaces.Commands;
using DemoCluster.GrainInterfaces.States;
using Microsoft.Extensions.Logging;
using Orleans.EventSourcing;
using Orleans.Providers;

namespace DemoCluster.GrainImplementations
{
    [LogConsistencyProvider(ProviderName="LogStorage")]
    [StorageProvider(ProviderName = "SqlBase")]
    public class MessageMonitorGrain :
        JournaledGrain<MessageMonitorState, IMessageMonitorCommand>,
        IMessageMonitorGrain
    {
        private readonly ILogger logger;
        //private bool isRunning = false;

        public MessageMonitorGrain(ILogger<MessageMonitorGrain> logger)
        {
            this.logger = logger;
        }

        public override async Task OnActivateAsync()
        {
            await RefreshNow();
        }

        public async Task<bool> UpdateMonitor(MonitorConfig config)
        {
            RaiseEvent(new MessageMonitorSetupCommand(config.Name, config.HostName, config.UserName, config.Password, config.ExchangeName, config.QueueName));
            await ConfirmEvents();

            return true;
        }

        public Task StartConsumer()
        {
            return Task.CompletedTask;
        }

        public Task StopConsumer()
        {
            return Task.CompletedTask;
        }
    }
}