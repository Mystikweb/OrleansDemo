using Orleans.Providers;
using OrleansDemo.Interfaces;
using OrleansDemo.Patterns.Registry;
using System;
using System.Threading.Tasks;

namespace OrleansDemo.Implementations
{
    [StorageProvider(ProviderName = "OrleansDemoStorage")]
    public class DeviceRegistryGrain : RegistryGrain<IDeviceGrain>, IDeviceRegistryGrain
    {
        public async Task<bool> HasDevice(Guid deviceId)
        {
            bool result = false;
            foreach (var device in State.RegisteredGrains)
            {
                var currentDeviceId = await device.GetDeviceId();

                if (currentDeviceId == deviceId)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
    }
}
