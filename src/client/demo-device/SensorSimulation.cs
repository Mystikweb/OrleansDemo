using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoDevice
{
    public class SensorSimulation
    {
        private readonly IModel model;
        private SensorConfig currentConfig = null;

        public SensorSimulation(IModel model)
        {
            this.model = model;
        }

        public Task<bool> StartAsync(SensorConfig config)
        {
            currentConfig = config;

            

            return Task.FromResult(true);
        }

        private void RunSimulator()
        {
            throw new NotImplementedException();
        }
    }
}
