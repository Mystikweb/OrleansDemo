using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Orleans.Providers;

namespace DemoCluster.Api
{
    public class DemoClusterApi : IBootstrapProvider
    {
        private IWebHost host;

        public string Name { get; private set; }

        public Task Init(string name, IProviderRuntime providerRuntime, IProviderConfiguration config)
        {
            Name = name;

            return Task.CompletedTask;
        }

        public Task Close()
        {
            throw new NotImplementedException();
        }
    }
}
