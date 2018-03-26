using DemoCluster.DAL;
using DemoCluster.GrainImplementations.Patterms;
using DemoCluster.GrainInterfaces;
using Orleans;
using Orleans.MultiCluster;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCluster.GrainImplementations
{
    [OneInstancePerCluster]
    [StorageProvider(ProviderName = "MemoryStorage")]
    public class DeviceRegistry : RegistryGrain<IDeviceGrain>, IDeviceRegistry
    {
        private IConfigurationStorage storage;

        public DeviceRegistry(IConfigurationStorage storage)
        {
            this.storage = storage;
        }

        public async Task Initialize()
        {
            var deviceList = await storage.GetDeviceListAsync();
            var activeDevices = deviceList.Where(d => d.IsEnabled).ToList();

            foreach (var device in activeDevices)
            {
                var deviceGrain = GrainFactory.GetGrain<IDeviceGrain>(Guid.Parse(device.DeviceId));
                await RegisterGrain(deviceGrain);

                await deviceGrain.Start(device);
            }
        }

        public async Task Teardown()
        {
            foreach (var device in State.RegisteredGrains)
            {
                await device.Stop();
            }
        }

        public async Task<bool> GetLoadedDeviceState(string deviceId)
        {
            bool result = false;

            var device = await storage.GetDeviceAsync(deviceId);
            var deviceGrain = GrainFactory.GetGrain<IDeviceGrain>(Guid.Parse(device.DeviceId));

            if (deviceGrain != null)
            {
                result = await deviceGrain.GetIsRunning();
            }

            return result;
        }

        public async Task StartDevice(string deviceId)
        {
            var device = await storage.GetDeviceAsync(deviceId);

            var deviceGrain = GrainFactory.GetGrain<IDeviceGrain>(Guid.Parse(device.DeviceId));
            await RegisterGrain(deviceGrain);

            await deviceGrain.Start(device);
        }

        public async Task StopDevice(string deviceId)
        {
            var device = await storage.GetDeviceAsync(deviceId);

            var deviceGrain = GrainFactory.GetGrain<IDeviceGrain>(Guid.Parse(device.DeviceId));
            await deviceGrain.Stop();
        }
    }
}
