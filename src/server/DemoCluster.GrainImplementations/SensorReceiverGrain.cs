using DemoCluster.DAL;
using DemoCluster.GrainInterfaces;
using DemoCluster.GrainInterfaces.States;
using Orleans;
using Orleans.Runtime;
using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoCluster.GrainImplementations
{
    public class SensorReceiverGrain : Grain, 
        IAsyncObserver<SensorMessage>, 
        ISensorReceiverGrain
    {
        private readonly IRuntimeStorage storage;

        private bool isReceving = false;
        private Logger logger;
        private IStreamProvider provider;
        private StreamSubscriptionHandle<SensorMessage> subscription;

        public SensorReceiverGrain(IRuntimeStorage storage)
        {
            this.storage = storage;
        }

        public override Task OnActivateAsync()
        {
            logger = GetLogger($"SensorMessageQueue_{this.GetPrimaryKeyLong()}");
            provider = GetStreamProvider("Rabbit");

            return base.OnActivateAsync();
        }

        public Task Initialiaze()
        {
            throw new NotImplementedException();
        }

        public Task<bool> StartReceiver()
        {
            throw new NotImplementedException();
        }

        public Task<bool> StopReceiver()
        {
            throw new NotImplementedException();
        }

        public Task OnCompletedAsync()
        {
            throw new NotImplementedException();
        }

        public Task OnErrorAsync(Exception ex)
        {
            throw new NotImplementedException();
        }

        public Task OnNextAsync(SensorMessage item, StreamSequenceToken token = null)
        {
            throw new NotImplementedException();
        }
    }
}
