using DemoCluster.DAL;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoCluster.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class RuntimeController : Controller
    {
        private readonly IRuntimeStorage runtime;
        private readonly IGrainFactory factory;
        private readonly Logger logger;

        public RuntimeController(IRuntimeStorage runtime, IGrainFactory factory, Logger logger)
        {
            this.runtime = runtime;
            this.factory = factory;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<DeviceState>> Get()
        {
            
            var configuredDevices = await runtime.GetDeviceStates();

            foreach (var device in configuredDevices)
            {
                try
                {
                    Task<bool> runningTask = Task.Run(async () =>
                    {
                        bool response = false;

                        var registry = factory.GetGrain<IDeviceRegistry>(0);
                        response = await registry.GetLoadedDeviceState(device.DeviceId);

                        return response;
                    });

                    device.IsRunning = await runningTask;
                }
                catch (Exception ex)
                {
                    logger.Error(1001, "Error calling Registry Grain.", ex);
                }
            }

            return configuredDevices;
        }

        [HttpPost("start/{deviceId}")]
        public async Task<IActionResult> PostStartDevice(string deviceId)
        {
            var registry = factory.GetGrain<IDeviceRegistry>(0);
            await registry.StartDevice(deviceId);

            return Ok();
        }
    }
}
