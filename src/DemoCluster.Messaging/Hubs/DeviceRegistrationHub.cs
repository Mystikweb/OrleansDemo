using DemoCluster.DAL.Logic;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Orleans;
using System;
using System.Threading.Tasks;

namespace DemoCluster.Messaging.Hubs
{
    public class DeviceRegistrationHub : Hub
    {
        private readonly ILogger logger;
        private readonly IClusterClient clusterClient;
        private readonly DeviceLogic deviceLogic;

        public DeviceRegistrationHub(ILogger<DeviceRegistrationHub> logger, IClusterClient clusterClient, DeviceLogic deviceLogic)
        {
            this.logger = logger;
            this.clusterClient = clusterClient;
            this.deviceLogic = deviceLogic;
        }

        public async Task RegisterDevice(string name)
        {
            DeviceConfig result = await deviceLogic.GetDeviceAsync(name, Context.ConnectionAborted);
            if (result != null)
            {
                IDeviceRegistry registry = clusterClient.GetGrain<IDeviceRegistry>(0);
                await registry.AddDevice(result);
                logger.LogInformation($"Deivce {name} was registered successfully.");
            }

            await Clients.Caller.SendAsync("RegisterDeviceResult", result);
        }

        public async Task UnregisterDevice(string deviceId)
        {
            DeviceConfig result = await deviceLogic.GetDeviceAsync(Guid.Parse(deviceId), Context.ConnectionAborted);
            if (result != null)
            {
                IDeviceRegistry registry = clusterClient.GetGrain<IDeviceRegistry>(0);
                await registry.RemoveDevice(result);
                logger.LogInformation($"Deivce {deviceId} was unregistered successfully.");
            }

            await Clients.Caller.SendAsync("UnregisterDeviceeResult", result);
        }

        public async Task StartDevice(string deviceId)
        {
            DeviceConfig result = await deviceLogic.GetDeviceAsync(Guid.Parse(deviceId), Context.ConnectionAborted);
            if (result != null)
            {
                IDeviceRegistry registry = clusterClient.GetGrain<IDeviceRegistry>(0);
                await registry.StartDevice(result.DeviceId);
                logger.LogInformation($"Deivce {deviceId} was started successfully.");
            }

            await Clients.Caller.SendAsync("StartDeviceResult", result);
        }

        public async Task StopDevice(string deviceId)
        {
            DeviceConfig result = await deviceLogic.GetDeviceAsync(Guid.Parse(deviceId), Context.ConnectionAborted);
            if (result != null)
            {
                IDeviceRegistry registry = clusterClient.GetGrain<IDeviceRegistry>(0);
                await registry.StopDevice(result.DeviceId);
                logger.LogInformation($"Deivce {deviceId} was stopped successfully.");
            }

            await Clients.Caller.SendAsync("StartDeviceResult", result);
        }
    }
}