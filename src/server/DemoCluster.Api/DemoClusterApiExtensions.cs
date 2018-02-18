
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
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
                { DemoClusterApiConstants.DEMOCLUSTER_API_HOSTNAME, options.HostName },
                { DemoClusterApiConstants.DEMOCLUSTER_API_PORT, options.Port.ToString() }
            };
        }

        public static ClusterConfiguration RegisterApi(this ClusterConfiguration config)
        {
            DemoClusterApiOptions options = new DemoClusterApiOptions();

            return config.RegisterApi(options);
        }

        public static ClusterConfiguration RegisterApi(this ClusterConfiguration config, DemoClusterApiOptions options)
        {
            config.Globals.RegisterBootstrapProvider<DemoClusterApi>("DemoApi", options.ToDictionary());

            return config;
        }

        public static ISiloHostBuilder UseApi(this ISiloHostBuilder builder)
        {
            builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(DemoClusterApi).Assembly));
            
            return builder;
        }
    }
}