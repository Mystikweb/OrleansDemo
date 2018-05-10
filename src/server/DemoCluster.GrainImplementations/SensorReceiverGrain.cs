using DemoCluster.DAL;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces;
using DemoCluster.GrainInterfaces.States;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;
using Orleans.Streams;
using System;
using System.Threading.Tasks;

namespace DemoCluster.GrainImplementations
{
    [StorageProvider(ProviderName = "MemoryStorage")]
    public class SensorReceiverGrain : Grain<SensorReceiverState>, 
        IAsyncObserver<SensorMessage>, 
        ISensorReceiverGrain
    {
        private readonly IRuntimeStorage storage;
        private readonly ILogger logger;

        private Guid streamId = Guid.NewGuid();
        private IStreamProvider provider;
        private StreamSubscriptionHandle<SensorMessage> subscription;

        public SensorReceiverGrain(IRuntimeStorage storage, ILogger<SensorReceiverGrain> logger)
        {
            this.storage = storage;
            this.logger = logger;
        }

        public override Task OnActivateAsync()
        {
            provider = GetStreamProvider("Rabbit");

            return base.OnActivateAsync();
        }

        public Task Initialize(SensorReceiverState state)
        {
            State = state;

            return Task.CompletedTask;
        }

        public Task<bool> IsReceiving()
        {
            return Task.FromResult(State.IsReceiving);
        }

        public async Task<bool> StartReceiver()
        {
            logger.Info($"Starting stream {streamId.ToString()} for {State.Device} - {State.Name}");
            var stream = provider.GetStream<SensorMessage>(streamId, $"{State.Device}_{State.Name}");

            subscription = await stream.SubscribeAsync(this);

            State.IsReceiving = true;

            return State.IsReceiving;
        }

        public async Task<bool> StopReceiver()
        {
            logger.Info($"Stopping stream {streamId.ToString()} for {State.Device} - {State.Name}");
            await subscription.UnsubscribeAsync();

            State.IsReceiving = false;

            return State.IsReceiving;
        }

        public Task OnCompletedAsync()
        {
            logger.Info($"Completed stream {streamId.ToString()} for {State.Device} - {State.Name}");
            return Task.CompletedTask;
        }

        public Task OnErrorAsync(Exception ex)
        {
            logger.Error(5002, $"Error on stream {streamId.ToString()} for {State.Device} - {State.Name}", ex);
            return Task.CompletedTask;
        }

        public async Task OnNextAsync(SensorMessage item, StreamSequenceToken token = null)
        {
            await storage.StoreSensorValue(new SensorValueItem
            {
                DeviceSensorId = item.DeviceSensorId,
                Value = item.Value,
                TimeStamp = DateTime.UtcNow
            });
        }
    }
}
