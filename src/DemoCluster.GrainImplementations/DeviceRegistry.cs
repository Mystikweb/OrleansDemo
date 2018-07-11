using DemoCluster.DAL;
using DemoCluster.DAL.Models;
using DemoCluster.GrainImplementations.Patterms;
using DemoCluster.GrainInterfaces;
using Microsoft.Extensions.Logging;
using Orleans.MultiCluster;
using Orleans.Providers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DemoCluster.GrainImplementations
{
    [OneInstancePerCluster]
    [StorageProvider(ProviderName = "CacheStorage")]
    public class DeviceRegistry : RegistryGrain<IDeviceGrain>, IDeviceRegistry
    {
        private readonly ILogger logger;
        private readonly IConfigurationStorage storage;

        public DeviceRegistry(ILogger<DeviceRegistry> logger, IConfigurationStorage storage)
        {
            this.logger = logger;
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

        public async Task<bool> GetLoadedDeviceState(string deviceId)
        {
            bool result = false;

            var device = await storage.GetDeviceByIdAsync(deviceId);
            var deviceGrain = GrainFactory.GetGrain<IDeviceGrain>(Guid.Parse(device.DeviceId));

            if (deviceGrain != null)
            {
                //var deviceState = await deviceGrain.GetCurrentState();
                result = true;
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
