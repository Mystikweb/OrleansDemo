using DemoCluster.DAL;
using DemoCluster.DAL.Models;
using DemoCluster.GrainImplementations.Patterms;
using DemoCluster.GrainInterfaces;
using DemoCluster.GrainInterfaces.States;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.MultiCluster;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoCluster.GrainImplementations
{
    [OneInstancePerCluster]
    [StorageProvider(ProviderName = "CacheStorage")]
    public class DeviceRegistry : RegistryGrain<IDeviceGrain>, IDeviceRegistry
    {
        private readonly ILogger logger;
        private readonly IGrainFactory factory;
        private readonly IConfigurationStorage storage;

        public DeviceRegistry(ILogger<DeviceRegistry> logger, IGrainFactory factory, IConfigurationStorage storage)
        {
            this.logger = logger;
            this.factory = factory;
            this.storage = storage;
        }

        public async Task Initialize()
        {
            var deviceList = await storage.GetDeviceListAsync();

            foreach (var device in deviceList)
            {
                var deviceGrain = GrainFactory.GetGrain<IDeviceGrain>(Guid.Parse(device.DeviceId));
                await RegisterGrain(deviceGrain);

                bool isSetup = await deviceGrain.UpdateConfig(device);
                if (!isSetup)
                {
                    logger.LogWarning($"Device {device.DeviceId} was not seupt correctly on initialization");
                }
            }
        }

        public async Task Teardown()
        {
            foreach (var device in State.RegisteredGrains)
            {
                var deviceConfig = await device.GetCurrentConfig();
                var stoppedState = deviceConfig.States.FirstOrDefault(s => s.IsEnabled && s.Name == "STOPPED");
                if (stoppedState != null)
                {
                    await device.UpdateCurrentStatus(stoppedState.ConfigToStateItem(Guid.Parse(deviceConfig.DeviceId)));
                }

                await UnregisterGrain(device);
            }
        }

        public async Task<List<DeviceState>> GetLoadedDeviceStates()
        {
            List<DeviceState> result = new List<DeviceState>();

            try
            {
                foreach (var device in State.RegisteredGrains)
                {
                    factory.BindGrainReference(device);
                    
                    var currentState = await device.GetState();
                    result.Add(currentState);
                }    
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, $"Error loading device states");
            }

            return result;
        }

        public async Task StartDevice(string deviceId)
        {
            var device = await storage.GetDeviceByIdAsync(deviceId);

            var deviceGrain = GrainFactory.GetGrain<IDeviceGrain>(Guid.Parse(device.DeviceId));
            await RegisterGrain(deviceGrain);

            //await deviceGrain.Start();
        }

        public async Task StopDevice(string deviceId)
        {
            var device = await storage.GetDeviceByIdAsync(deviceId);

            var deviceGrain = GrainFactory.GetGrain<IDeviceGrain>(Guid.Parse(device.DeviceId));
            //await deviceGrain.Stop();
        }
    }
}
