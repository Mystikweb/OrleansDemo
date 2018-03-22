using Orleans.Runtime.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCluster.GrainImplementations
{
    public static class GrainImplementationExtensions
    {
        public static ClusterConfiguration RegisterBootstrapGrains(this ClusterConfiguration config)
        {
            config.Globals.RegisterBootstrapProvider<DeviceRegistryBootstrap>("RegistryBootstrap");

            return config;
        }
    }
}
