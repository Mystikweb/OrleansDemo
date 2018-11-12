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
        private readonly IGrainFactory grainFactory;
        private readonly IHubContext<DeviceHub> deviceHub;

        public DashboardController(ILogger<DashboardController> logger,
            IGrainFactory grainFactory,
            IHubContext<DeviceHub> deviceHub)
        {
            this.logger = logger;
            this.grainFactory = grainFactory;
            this.deviceHub = deviceHub;
        }

        public async Task<ActionResult<List<DeviceSummaryViewModel>>> GetAsync()
        {
            IDeviceServiceGrain serviceGrain = grainFactory.GetGrain<IDeviceServiceGrain>(0);

            return Ok(await serviceGrain.GetDevices());
        }
    }
}