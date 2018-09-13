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
                await registry.StartDevice(result.DeviceId);
            }

            await Clients.Caller.SendAsync("ReceiveConfig", result);
        }
    }
}