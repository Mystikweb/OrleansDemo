using DemoCluster.GrainInterfaces;
using Orleans.Providers;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoCluster.GrainImplementations
{
    public class DeviceRegistryBootstrap : IBootstrapProvider
    {
        private IDeviceRegistry registry;
        private Logger logger;

        public string Name { get; private set; }

        public async Task Init(string name, IProviderRuntime providerRuntime, IProviderConfiguration config)
        {
            Name = name;
            logger = providerRuntime.GetLogger(name);
            registry = providerRuntime.GrainFactory.GetGrain<IDeviceRegistry>(0);

            logger.Info("Initializing the device registry");
            await registry.Initialize();
        }

        public async Task Close()
        {
            await registry.Teardown();
            logger.Info("Device registry teardown complete");
        }
    }
}
