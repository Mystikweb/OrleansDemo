using DemoCluster.DAL.Logic;
using DemoCluster.GrainInterfaces;
using DemoCluster.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Orleans;
using System.Threading.Tasks;

namespace DemoCluster.Runtime.Hubs
{
    public class RegistrationHub : Hub
    {
        private readonly ILogger logger;
        private readonly IClusterClient clusterClient;
        private readonly DeviceLogic deviceLogic;

        public RegistrationHub(ILogger<RegistrationHub> logger,
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

            DeviceViewModel result = await deviceLogic.GetDeviceAsync(name);
            if (result != null)
            {
                IDeviceServiceGrain serviceGrain = clusterClient.GetGrain<IDeviceServiceGrain>(0);
                logger.LogDebug($"Device registry invoked and making call to start device {result.Name} with id {result.DeviceId}");
                await serviceGrain.AddDevice(result);
            }

            await Clients.Caller.SendAsync(Constants.MESSAGING_REGISTRATION_RECEIVE_CONFIG, result);
        }
    }
}
