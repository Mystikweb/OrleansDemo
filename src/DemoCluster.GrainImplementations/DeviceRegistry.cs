using DemoCluster.DAL;
using DemoCluster.DAL.Logic;
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
        private readonly DeviceLogic deviceLogic;

        public DeviceRegistry(ILogger<DeviceRegistry> logger, DeviceLogic deviceLogic)
        {
            this.logger = logger;
            this.deviceLogic = deviceLogic;
        }

        public async Task Initialize()
        {
            List<DeviceConfig> deviceList = await deviceLogic.GetDeviceListAsync(d => d.IsEnabled);

            foreach (DeviceConfig device in deviceList)
            {
                IDeviceGrain deviceGrain = GrainFactory.GetGrain<IDeviceGrain>(Guid.Parse(device.DeviceId));
                await RegisterGrain(deviceGrain);

                bool isSetup = await deviceGrain.UpdateConfig(device);
                if (isSetup)
                {
                    logger.LogInformation($"Device {device.DeviceId} has been registered on initialization");
                }
                else
                {
                    logger.LogError($"Device {device.DeviceId} was not registered correctly on initialization");
                }
            }
        }

        public async Task Teardown()
        {
            foreach (var device in State.RegisteredGrains)
            {
                DeviceConfig deviceConfig = await device.GetCurrentConfig();
                DeviceStateConfig stoppedState = deviceConfig.States.FirstOrDefault(s => s.IsEnabled && s.Name == "STOPPED");
                if (stoppedState != null)
                {
                    logger.LogInformation($"Updating state of device {deviceConfig.DeviceId} to STOPPED");
                    await device.UpdateCurrentStatus(stoppedState.ConfigToStateItem(Guid.Parse(deviceConfig.DeviceId)));
                }

                await UnregisterGrain(device);
                logger.LogInformation($"Unregistered device {deviceConfig.DeviceId}");
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
            catch (System.Exception ex)
            {
                logger.LogError(ex, $"Error loading device states");
            }

            return result;
        }

        public async Task AddDevice(DeviceConfig config)
        {
            IDeviceGrain device = GrainFactory.GetGrain<IDeviceGrain>(Guid.Parse(config.DeviceId));
            
            bool isSetup = await device.UpdateConfig(config);
            await RegisterGrain(device);

            if (isSetup)
            {
                logger.LogInformation($"Device {config.DeviceId} has been registered");
            }
            else
            {
                logger.LogWarning($"Device {config.DeviceId} was not registered correctly");
            }
        }

        public async Task RemoveDevice(DeviceConfig config)
        {
            IDeviceGrain device = GrainFactory.GetGrain<IDeviceGrain>(Guid.Parse(config.DeviceId));

            DeviceConfig deviceConfig = await device.GetCurrentConfig();
            DeviceStateConfig stoppedState = deviceConfig.States.FirstOrDefault(s => s.IsEnabled && s.Name == "STOPPED");
            if (stoppedState != null)
            {
                logger.LogInformation($"Updating state of device {deviceConfig.DeviceId} to STOPPED");
                await device.UpdateCurrentStatus(stoppedState.ConfigToStateItem(Guid.Parse(deviceConfig.DeviceId)));
            }

            await UnregisterGrain(device);
            logger.LogInformation($"Unregistered device {deviceConfig.DeviceId}");
        }

        public async Task StartDevice(string deviceId)
        {
            IDeviceGrain currentDevice = null;

            List<IDeviceGrain> deviceList = await GetRegisteredGrains();
            foreach (IDeviceGrain device in deviceList)
            {
                if (Guid.Parse(deviceId) == device.GetPrimaryKey())
                {
                    currentDevice = device;
                }
            }

            if (currentDevice != null)
            {
                // start device here
            }
            else
            {
                logger.LogError($"Device {deviceId} was not found in the registered device list");
            }
        }

        public async Task StopDevice(string deviceId)
        {
            IDeviceGrain currentDevice = null;

            List<IDeviceGrain> deviceList = await GetRegisteredGrains();
            foreach (IDeviceGrain device in deviceList)
            {
                if (Guid.Parse(deviceId) == device.GetPrimaryKey())
                {
                    currentDevice = device;
                }
            }

            if (currentDevice != null)
            {
                // stop device here
            }
            else
            {
                logger.LogError($"Device {deviceId} was not found in the registered device list");
            }
        }
    }
}
