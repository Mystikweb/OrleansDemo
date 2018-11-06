using DemoCluster.DAL.Logic;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Orleans;
using System.Threading.Tasks;

namespace DemoCluster.Messaging.Hubs
{
    public class DeviceRegistrationHub : Hub
    {
        private readonly ILogger logger;
        private readonly IClusterClient clusterClient;
        private readonly DeviceLogic deviceLogic;

        public DeviceRegistrationHub(ILogger<DeviceRegistrationHub> logger, 
            IClusterClient clusterClient, 
            DeviceLogic deviceLogic)
        {
            this.logger = logger;
            this.clusterClient = clusterClient;
            this.deviceLogic = deviceLogic;
        }

        public async Task RegisterDevice(string name)
        {
            logger.LogInformation($"Received registration for device {name}");

            DeviceConfig result = await deviceLogic.GetDeviceAsync(name);
            if (result != null)
            {
                IDeviceRegistry registry = clusterClient.GetGrain<IDeviceRegistry>(0);
                logger.LogDebug($"Device registry invoked and making call to start device {result.Name} with id {result.DeviceId}");
                await registry.StartDevice(result.DeviceId);
                
            }

            await Clients.Caller.SendAsync("ReceiveConfig", result);
        }
    }
}