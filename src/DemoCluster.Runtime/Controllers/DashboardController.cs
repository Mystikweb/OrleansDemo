using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoCluster.GrainInterfaces;
using DemoCluster.Models;
using DemoCluster.Runtime.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Orleans;

namespace DemoCluster.Runtime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IClusterClient clusterClient;
        private readonly IHubContext<DeviceHub> deviceHub;

        public DashboardController(ILogger<DashboardController> logger,
            IClusterClient clusterClient,
            IHubContext<DeviceHub> deviceHub)
        {
            this.logger = logger;
            this.clusterClient = clusterClient;
            this.deviceHub = deviceHub;
        }

        [HttpGet]
        public async Task<List<DeviceSummaryViewModel>> Get()
        {
            IDeviceServiceGrain serviceGrain = clusterClient.GetGrain<IDeviceServiceGrain>(0);

            return await serviceGrain.GetDevices();
        }
    }
}