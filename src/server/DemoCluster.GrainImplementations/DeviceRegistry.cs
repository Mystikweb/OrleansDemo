using DemoCluster.DAL;
using DemoCluster.DAL.States;
using DemoCluster.GrainImplementations.Patterms;
using DemoCluster.GrainInterfaces;
using Orleans;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCluster.GrainImplementations
{
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
                var deviceGrain = GrainFactory.GetGrain<IDeviceGrain>(device.DeviceId);
                await deviceGrain.Start();

                await RegisterGrain(deviceGrain);
            }
        }
    }
}
