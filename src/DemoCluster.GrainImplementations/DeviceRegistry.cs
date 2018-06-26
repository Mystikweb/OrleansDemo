using DemoCluster.DAL;
using DemoCluster.GrainImplementations.Patterms;
using DemoCluster.GrainInterfaces;
using Orleans.MultiCluster;
using Orleans.Providers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DemoCluster.GrainImplementations
{
    [OneInstancePerCluster]
    [StorageProvider(ProviderName = "MemoryStorage")]
    public class DeviceRegistry : RegistryGrain<IDeviceWorkerGrain>, IDeviceRegistry
    {
        private IConfigurationStorage storage;

        public DeviceRegistry(IConfigurationStorage storage)
        {
            this.storage = storage;
        }

        public async Task Initialize()
        {
            var deviceList = await storage.GetDeviceListAsync();

            foreach (var device in deviceList)
            {
                var deviceGrain = GrainFactory.GetGrain<IDeviceWorkerGrain>(Guid.Parse(device.DeviceId));
                await RegisterGrain(deviceGrain);

                if (device.IsEnabled)
                {
                    await deviceGrain.Start();
                }
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

            var device = await storage.GetDeviceByIdAsync(deviceId);
            var deviceGrain = GrainFactory.GetGrain<IDeviceWorkerGrain>(Guid.Parse(device.DeviceId));

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

            var deviceGrain = GrainFactory.GetGrain<IDeviceWorkerGrain>(Guid.Parse(device.DeviceId));
            await RegisterGrain(deviceGrain);

            await deviceGrain.Start();
        }

        public async Task StopDevice(string deviceId)
        {
            var device = await storage.GetDeviceByIdAsync(deviceId);

            var deviceGrain = GrainFactory.GetGrain<IDeviceWorkerGrain>(Guid.Parse(device.DeviceId));
            await deviceGrain.Stop();
        }
    }
}
