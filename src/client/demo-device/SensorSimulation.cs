using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemoDevice
{
    public class SensorSimulation
    {
        internal class SensorMessage
        {
            public int DeviceSensorId { get; private set; }
            public double Value { get; private set; }

            public SensorMessage(int deviceSensorId, double value)
            {
                DeviceSensorId = deviceSensorId;
                Value = value;
            }

            public byte[] ToRabbitMessage()
            {
                return Encoding.UTF8.GetBytes(ToJson());
            }

            public string ToJson()
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(this);
            }

            public override string ToString()
            {
                return $"Sending {Value} to {DeviceSensorId}";
            }
        }

        private CancellationTokenSource cancellationToken = new CancellationTokenSource();
        private readonly IModel model;
        private readonly StreamConfiguration streamConfig;
        private SensorConfig currentConfig = null;

        public SensorSimulation(IModel model, StreamConfiguration streamConfig)
        {
            this.model = model;
            this.streamConfig = streamConfig;
        }

        public Task<bool> StartAsync(SensorConfig config)
        {
            currentConfig = config;

            Task.Run(RunSimulator);

            return Task.FromResult(true);
        }

        public Task<bool> StopAsync()
        {
            cancellationToken.Cancel();

            return Task.FromResult(true);
        }

        private async Task RunSimulator()
        {
            Random generator = new Random();

            while (!cancellationToken.IsCancellationRequested)
            {
                double value = generator.NextDouble();

                var message = new SensorMessage(currentConfig.DeviceSensorId, value);

                var props = model.CreateBasicProperties();

                model.BasicPublish(streamConfig.Exchange,
                    streamConfig.SensorQueue,
                    props,
                    message.ToRabbitMessage());

                Console.WriteLine(message.ToString());
                await Task.Delay(5000);
            }
        }
    }
}
