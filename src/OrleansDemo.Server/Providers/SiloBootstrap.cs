using Microsoft.Extensions.DependencyInjection;
using Orleans.Providers;
using Orleans.Runtime;
using OrleansDemo.Interfaces;
using OrleansDemo.Models.Runtime;
using OrleansDemo.Models.Transfer;
using OrleansDemo.Server.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrleansDemo.Server.Providers
{
    public class SiloBootstrap : IBootstrapProvider
    {
        private IConfigurationManager configManager;
        private RuntimeContext runtime;
        private IDeviceRegistryGrain deviceRegistry;
        private Logger logger;

        public string Name { get; private set; }

        public Task Close()
        {
            return Task.CompletedTask;
        }

        public async Task Init(string name, IProviderRuntime providerRuntime, IProviderConfiguration config)
        {
            Name = name;
            logger = providerRuntime.GetLogger(name);

            runtime = providerRuntime.ServiceProvider.GetService<RuntimeContext>();
            configManager = providerRuntime.ServiceProvider.GetService<IConfigurationManager>();
            deviceRegistry = providerRuntime.GrainFactory.GetGrain<IDeviceRegistryGrain>(Constants.DeviceRegistryId);

            List<DeviceConfiguration> configurations = await configManager.GetDeviceConfigurations();
            foreach (DeviceConfiguration configuredDevice in configurations)
            {
                //bool deviceExists = await deviceRegistry.HasDevice(configuredDevice.DeviceId);
                //if (!deviceExists)
                //{
                //    var device = providerRuntime.GrainFactory.GetGrain<IDeviceGrain>(configuredDevice.DeviceId);
                //    await device.Initialize(configuredDevice);

                //    await deviceRegistry.RegisterGrain(device);
                //}
            }
        }
    }
}
