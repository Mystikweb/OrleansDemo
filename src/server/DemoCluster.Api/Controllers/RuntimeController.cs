using DemoCluster.DAL;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces;
using DemoCluster.GrainInterfaces.States;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoCluster.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class RuntimeController : Controller
    {
        private readonly IRuntimeStorage runtime;
        private readonly IActionDispatcher dispatcher;
        private readonly IGrainFactory factory;
        private readonly ILogger logger;

        public RuntimeController(IRuntimeStorage runtime, 
            IActionDispatcher dispatcher,
            IGrainFactory factory, 
            ILogger<RuntimeController> logger)
        {
            this.runtime = runtime;
            this.dispatcher = dispatcher;
            this.factory = factory;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<DeviceState>> Get()
        {
            var registry = factory.GetGrain<IDeviceRegistry>(0);

            var devices = await dispatcher.DispatchAsync(() => registry.GetRegisteredGrains()).ConfigureAwait(false);

            var result = new List<DeviceState>();

            // foreach (var device in devices)
            // {
            //     var currentState = await dispatcher.DispatchAsync(() => device.GetCurrentState()).ConfigureAwait(false);
            //     result.Add(currentState);
            // }

            return result;
        }

        [HttpPost("start")]
        public async Task<IActionResult> PostStartDevice([FromBody] DeviceStatusChange device)
        {
            var registry = factory.GetGrain<IDeviceRegistry>(0);

            try
            {
                await dispatcher.DispatchAsync(() => registry.StartDevice(device.DeviceId)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.Error(1001, "Error starting device grain.", ex);
                return BadRequest(ex);
            }

            return Ok();
        }

        [HttpPost("stop")]
        public async Task<IActionResult> PostStopDevice([FromBody] DeviceStatusChange device)
        {
            var registry = factory.GetGrain<IDeviceRegistry>(0);

            try
            {
                await dispatcher.DispatchAsync(() => registry.StopDevice(device.DeviceId)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.Error(1001, "Error starting device grain.", ex);
                return BadRequest(ex);
            }

            return Ok();
        }
    }
}
