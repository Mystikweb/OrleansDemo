using DemoCluster.DAL;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces;
using DemoCluster.GrainInterfaces.States;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoCluster.GrainImplementations
{
    [StorageProvider(ProviderName = "MemoryStorage")]
    public class SensorGrain : Grain<SensorState>, ISensorGrain
    {
        private readonly IRuntimeStorage storage;

        private Logger logger;
        private ISensorReceiverGrain receiver;

        public SensorGrain(IRuntimeStorage storage)
        {
            this.storage = storage;
        }

        public override Task OnActivateAsync()
        {
            logger = GetLogger($"Sensor_{this.GetPrimaryKeyLong()}");
            receiver = GrainFactory.GetGrain<ISensorReceiverGrain>(this.GetPrimaryKeyLong());

            return base.OnActivateAsync();
        }

        public Task Initialize(DeviceSensorConfig config, string deviceName)
        {
            State = config.ToSensorState();
            receiver.Initialize(config.ToReceiverState(deviceName));

            return Task.CompletedTask;
        }

        public async Task<bool> GetIsReceiving()
        {
            return await receiver.IsReceiving();
        }

        public async Task StartReceiving()
        {
            bool didStart = await receiver.StartReceiver();
        }

        public async Task StopReceiving()
        {
            bool didStop = await receiver.StopReceiver();
        }
    }
}
