using System;
using System.Collections.Generic;
using System.Net.Http;
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
        private EventingBasicConsumer consumer = null;

        private DeviceConfig currentConfig = null;
        private string endpoint = string.Empty;
        private Action<string> log = null;
        private HashSet<SensorSimulation> sensorList = new HashSet<SensorSimulation>();

        public bool CanRun { get; private set; }

        public DeviceHost(StreamConfiguration streamConfig, string endpoint, Action<string> log)
        {
            this.streamConfig = streamConfig;
            this.endpoint = endpoint;
            this.log = log;

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

        public async Task GetDeviceConfigAsync(string name)
        {
            using (var http = new HttpClient())
            {
                string response = await http.GetStringAsync($"{endpoint}/device/name/{name}");

                if (string.IsNullOrEmpty(response))
                {
                    log?.Invoke($"Unable to locate {name}, has it been configured yet?");
                    CanRun = false;
                }
                else
                {
                    currentConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<DeviceConfig>(response);
                    log?.Invoke("Config availble and ready to start");
                    CanRun = true;
                }
            }
        }

        public async Task StartAsync()
        {
            model.ExchangeDeclare(streamConfig.Exchange, streamConfig.ExchangeType);
            model.QueueDeclare(streamConfig.DeviceQueue);

            model.QueueBind(queue: streamConfig.DeviceQueue,
                            exchange: streamConfig.Exchange,
                            routingKey: streamConfig.RoutingKey);

            consumer = new EventingBasicConsumer(model);
            consumer.Received += MessageReceived;
            
            model.BasicConsume(streamConfig.DeviceQueue, false, consumer);

            await StartSensorsAsync();
        }

        public async Task StopAsync()
        {
            foreach (var simulator in sensorList)
            {
                await simulator.StopAsync();
            }

            model.Dispose();
            connection.Dispose();
        }

        private async Task StartSensorsAsync()
        {
            foreach (var sensor in currentConfig.Sensors)
            {
                var simulator = new SensorSimulation(model, streamConfig);
                await simulator.StartAsync(sensor);

                sensorList.Add(simulator);
            }
        }

        private void MessageReceived(object sender, BasicDeliverEventArgs ea)
        {
            log?.Invoke("Received new message");

            model.BasicAck(ea.DeliveryTag, false);
        }
    }
}