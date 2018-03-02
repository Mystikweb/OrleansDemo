using DemoCluster.DAL;
using DemoCluster.DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoCluster.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DeviceController : Controller
    {
        private readonly IConfigurationStorage configurationStorage;

        public DeviceController(IConfigurationStorage configStore)
        {
            configurationStorage = configStore;
        }

        [HttpGet]
        public async Task<IEnumerable<DeviceConfig>> Get()
        {
            return await configurationStorage.GetDeviceListAsync();
        }

        [HttpGet("{deviceId}")]
        public async Task<IActionResult> Get(Guid deviceId)
        {
            return Ok(await configurationStorage.GetDeviceAsync(deviceId));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DeviceConfig config)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                
            DeviceConfig device = await configurationStorage.SaveDeviceAsync(config);

            return CreatedAtAction("GetDevice", new { id = device.DeviceId }, device);
        }
    }
}