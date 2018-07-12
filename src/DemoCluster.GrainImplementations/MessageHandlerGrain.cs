using System;
using System.Threading.Tasks;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orleans;
using Orleans.Concurrency;

namespace DemoCluster.GrainImplementations
{
    [StatelessWorker(5)]
    public class MessageHandlerGrain :
        Grain, IMessageHandlerGrain
    {
        private readonly ILogger logger;

        public MessageHandlerGrain(ILogger<MessageHandlerGrain> logger)
        {
            this.logger = logger;
        }

        public async Task RouteMessage(string message)
        {
            try
            {
                SensorValueItem deserialized = JsonConvert.DeserializeObject<SensorValueItem>(message);

                var sensorGrain = GrainFactory.GetGrain<ISensorGrain>(deserialized.DeviceSensorId);
                await sensorGrain.RecordValue(deserialized);

                logger.LogInformation($"Routed message '{message}' to device sensor id {deserialized.DeviceSensorId}'");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error routing message '{message}'");
            }
        }
    }
}