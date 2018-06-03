using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace DemoCluster.Util.Messaging
{
    public class RabbitConnection : IRabbitConnection
    {
        private readonly IConnection connection;
        private readonly string DefaultExchange = string.Empty;
        private readonly string DefaultNamespace = string.Empty;

        public RabbitConnection(string host, int port, string user, string password, string vhost, string ns, string exchange)
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = host,
                Port = port,
                UserName = user,
                Password = password,
                VirtualHost = "/"
            };

            DefaultNamespace = ns;
            DefaultExchange = exchange;

            connection = connectionFactory.CreateConnection();
        }

        public IModel GetChannel()
        {
            return connection.CreateModel();
        }

        public IModel CreateConsumer(string queue, string topic, EventHandler<BasicDeliverEventArgs> receivedCallback)
        {
            var channel = connection.CreateModel();
            channel.ExchangeDeclare(DefaultExchange, "topic", true, true);
            channel.QueueBind(queue, DefaultExchange, topic);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += receivedCallback;

            channel.BasicConsume(consumer, queue, true);

            return channel;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    connection.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~RabbitConnection() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
