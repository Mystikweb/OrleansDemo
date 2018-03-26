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
    public class SensorMessageJournal : Grain, 
        IAsyncObserver<SensorMessage>, 
        ISensorMessageJournal
    {
        private Logger logger;
        private IStreamProvider provider;
        private StreamSubscriptionHandle<SensorMessage> subscription;

        public override Task OnActivateAsync()
        {
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
