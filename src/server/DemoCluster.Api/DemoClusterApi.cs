using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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

            IOptions<DemoClusterApiOptions> options = providerRuntime.ServiceProvider.GetRequiredService<IOptions<DemoClusterApiOptions>>();

            if (options.Value.InternalHost)
            {
                host = new WebHostBuilder()
                    .ConfigureServices(services =>
                    {
                        
                    })
                    .Configure(app =>
                    {
                        app.UseMvc();
                    })
                    .UseKestrel()
                    .UseUrls($"http://{options.Value.HostName}:{options.Value.Port}")
                    .Build();
            }

            return Task.CompletedTask;
        }

        public Task Close()
        {
            throw new NotImplementedException();
        }
    }
}
