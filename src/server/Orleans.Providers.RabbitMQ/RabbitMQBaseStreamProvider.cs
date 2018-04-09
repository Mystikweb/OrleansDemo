using Orleans.Providers.Streams.Common;

namespace Orleans.Providers.RabbitMQ
{
    public abstract class RabbitMQBaseStreamProvider<TMapper> : PersistentStreamProvider<RabbitMQAdapterFactory<TMapper>> where TMapper : IRabbitMQMapper
    {
    }
}
