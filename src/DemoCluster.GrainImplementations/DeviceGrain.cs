using System.Threading.Tasks;
using DemoCluster.DAL;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces;
using DemoCluster.GrainInterfaces.States;
using Orleans;
using Orleans.Providers;

namespace DemoCluster.GrainImplementations
{
    [StorageProvider(ProviderName = "CacheStorage")]
    public class DeviceGrain : Grain<DeviceState>, IDeviceGrain
    {
        private readonly IConfigurationStorage configuration;

        private DeviceConfig deviceConfig;

        public DeviceGrain()
        {
            
        }

        public Task<DeviceConfig> GetCurrentConfig()
        {
            throw new System.NotImplementedException();
        }

        public Task<DeviceStateItem> GetCurrentStatus()
        {
            throw new System.NotImplementedException();
        }

        public Task Start()
        {
            throw new System.NotImplementedException();
        }

        public Task Stop()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateConfig()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateCurrentStatus(DeviceStateItem state)
        {
            throw new System.NotImplementedException();
        }
    }
}