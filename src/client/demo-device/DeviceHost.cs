
using System;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DemoDevice
{
    public class DeviceHost
    {
        private readonly StreamConfiguration streamConfig;
        private readonly IConnection connection;
        private readonly IModel model;
        private Action<string> log = null;
        private EventingBasicConsumer consumer = null;
        private DeviceConfig currentConfig = null;

        public DeviceHost(StreamConfiguration streamConfig)
        {
            this.streamConfig = streamConfig;

            var factory = new ConnectionFactory()
            {
                HostName = this.streamConfig.HostName,
                VirtualHost = this.streamConfig.VirtualHost,
                UserName = this.streamConfig.Username,
                Password = this.streamConfig.Password
            };

            connection = factory.CreateConnection();
            model = connection.CreateModel();
        }

        public Task StartAsync(Action<string> log)
        {
            this.log = log;

            model.ExchangeDeclare(streamConfig.Exchange, streamConfig.ExchangeType);
            model.QueueDeclare(streamConfig.Queue);

            model.QueueBind(queue: streamConfig.Queue,
                            exchange: streamConfig.Exchange,
                            routingKey: streamConfig.RoutingKey);

            consumer = new EventingBasicConsumer(model);
            consumer.Received += MessageReceived;
            
            model.BasicConsume(streamConfig.Queue, true, consumer);

            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            model.Dispose();
            connection.Dispose();

            return Task.CompletedTask;
        }

        private Task StartSensorsAsync()
        {

            return Task.CompletedTask;
        }

        private void MessageReceived(object model, BasicDeliverEventArgs ea)
        {
            log?.Invoke("Received new message");
        }
    }
}