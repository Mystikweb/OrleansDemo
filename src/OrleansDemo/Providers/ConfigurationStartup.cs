using Orleans.Providers;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrleansDemo.Providers
{
    public class ConfigurationStartup : IBootstrapProvider
    {
        private Logger logger;

        public string Name { get; private set; }

        public Task Close()
        {
            return Task.CompletedTask;
        }

        public Task Init(string name, IProviderRuntime providerRuntime, IProviderConfiguration config)
        {
            Name = name;

            logger = providerRuntime.GetLogger(name);

            return Task.CompletedTask;
        }
    }
}
