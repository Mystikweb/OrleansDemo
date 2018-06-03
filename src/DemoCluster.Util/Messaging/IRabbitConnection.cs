
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace DemoCluster.Util.Messaging
{
    public interface IRabbitConnection : IDisposable
    {
        IModel GetChannel();
        IModel CreateConsumer(string queue, string topic, EventHandler<BasicDeliverEventArgs> receivedCallback);
    }
}