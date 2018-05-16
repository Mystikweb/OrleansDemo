
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Hosting;
using Orleans.Runtime.Configuration;

namespace DemoCluster.Api
{
    public static class DemoClusterApiExtensions
    {
        public static Dictionary<string, string> ToDictionary(this DemoClusterApiOptions options)
        {
            return new Dictionary<string, string>
            {
                { DemoClusterApiConstants.DEMOCLUSTER_RUNTIME_CONNNECTIONSTRING, options.RuntimeConnnectionString },
                { DemoClusterApiConstants.DEMOCLUSTER_API_HOSTNAME, options.HostName },
                { DemoClusterApiConstants.DEMOCLUSTER_API_PORT, options.Port.ToString() }
            };
        }

        public static ISiloHostBuilder UseApi(this ISiloHostBuilder builder, Action<DemoClusterApiOptions> options = null)
        {
            builder.ConfigureServices(services =>
            {
                if (options != null)
                {
                    services.Configure(options);
                }
                else
                {
                    services.Configure<DemoClusterApiOptions>(config => new DemoClusterApiOptions());
                }
            });

            builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(DemoClusterApi).Assembly));
            builder.AddStartupTask<DemoClusterApi>();

            return builder;
        }
    }
}