using DemoCluster.DAL.Logic;
using DemoCluster.GrainInterfaces;
using DemoCluster.Models;
using DemoCluster.States;
using Microsoft.Extensions.Logging;
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
        private readonly DeviceLogic storage;

        public DeviceRegistry(ILogger<DeviceRegistry> logger, DeviceLogic storage)
        {
            this.logger = logger;
            this.storage = storage;
        }

        public async Task Initialize()
        {
            List<DeviceViewModel> deviceList = await storage.GetDeviceListAsync();

            foreach (DeviceViewModel deviceModel in deviceList)
            {
                IDeviceGrain deviceGrain = GrainFactory.GetGrain<IDeviceGrain>(Guid.Parse(deviceModel.DeviceId));
                await RegisterGrain(deviceGrain);

                bool isSetup = await deviceGrain.UpdateDevice(deviceModel);
                if (!isSetup)
                {
                    logger.LogWarning($"Device {deviceModel.DeviceId} was not seupt correctly on initialization");
                }
            }
        }

        public async Task Teardown()
        {
            foreach (IDeviceGrain device in State.RegisteredGrains)
            {
                DeviceViewModel deviceModel = await device.GetDeviceModel();
                DeviceStateViewModel stoppedState = deviceModel.States.FirstOrDefault(s => s.IsEnabled && s.StateName == "STOPPED");
                if (stoppedState != null)
                {
                    await device.UpdateDeviceState(stoppedState);
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
                    GrainFactory.BindGrainReference(device);
                    
                    var currentState = await device.GetState();
                    result.Add(currentState);
                }    
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error loading device states");
            }

            return result;
        }

        public async Task StartDevice(string deviceId)
        {
            var device = await storage.GetDeviceAsync(Guid.Parse(deviceId));

            var deviceGrain = GrainFactory.GetGrain<IDeviceGrain>(Guid.Parse(device.DeviceId));
            await RegisterGrain(deviceGrain);

            //await deviceGrain.Start();
        }

        public async Task StopDevice(string deviceId)
        {
            var device = await storage.GetDeviceAsync(Guid.Parse(deviceId));

            var deviceGrain = GrainFactory.GetGrain<IDeviceGrain>(Guid.Parse(device.DeviceId));
            //await deviceGrain.Stop();
        }
    }
}
