using DemoCluster.DAL;
using DemoCluster.DAL.Models;
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
        private readonly IConfigurationStorage storage;

        public DeviceController(IConfigurationStorage storage)
        {
            this.storage = storage;
        }

        [HttpGet]
        public async Task<IEnumerable<DeviceConfig>> Get()
        {
            return await storage.GetDeviceListAsync();
        }

        [HttpGet("{deviceId}")]
        public async Task<IActionResult> GetById(string deviceId)
        {
            return Ok(await storage.GetDeviceByIdAsync(deviceId));
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            return Ok(await storage.GetDeviceByNameAsync(name));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DeviceConfig config)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                
            DeviceConfig device = await storage.SaveDeviceAsync(config);

            return CreatedAtAction("Get", new { deviceId = device.DeviceId }, device);
        }

        [HttpDelete("{deviceId}")]
        public async Task<IActionResult> Delete(string deviceId)
        {
            await storage.RemoveDeviceAsync(deviceId);
            return Ok();
        }
    }
}