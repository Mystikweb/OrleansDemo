using DemoCluster.DAL;
using DemoCluster.DAL.Logic;
using DemoCluster.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoCluster.Configuration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DeviceController : Controller
    {
        private readonly DeviceLogic deviceLogic;

        public DeviceController(DeviceLogic deviceLogic)
        {
            this.deviceLogic = deviceLogic;
        }

        [HttpGet]
        public async Task<IEnumerable<DeviceConfig>> Get()
        {
            return await deviceLogic.GetDeviceListAsync(HttpContext.RequestAborted);
        }

        [HttpGet("{deviceId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<DeviceConfig>> GetById(string deviceId)
        {
            DeviceConfig result = await deviceLogic.GetDeviceAsync(Guid.Parse(deviceId), HttpContext.RequestAborted);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("name/{name}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<DeviceConfig>> GetByName(string name)
        {
            DeviceConfig result = await deviceLogic.GetDeviceAsync(name, HttpContext.RequestAborted);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        public async Task<ActionResult<DeviceConfig>> Post([FromBody] DeviceConfig config)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            DeviceConfig result = null;

            try
            {
                result = await deviceLogic.CreateDeviceAsync(config, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return CreatedAtAction(nameof(GetById), new { deviceId = result.DeviceId }, result);
        }

        [HttpPut("{deviceId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> Put(string deviceId, [FromBody] DeviceConfig config)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DeviceConfig device = await deviceLogic.GetDeviceAsync(Guid.Parse(deviceId), HttpContext.RequestAborted);
            if (device == null)
            {
                return NotFound();
            }

            try
            {
                await deviceLogic.UpdateDeviceAsync(config, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return NoContent();
        }

        [HttpDelete("{deviceId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> Delete(string deviceId)
        {
            DeviceConfig config = await deviceLogic.GetDeviceAsync(Guid.Parse(deviceId), HttpContext.RequestAborted);
            if (config == null)
            {
                return NotFound();
            }

            try
            {
                await deviceLogic.DeleteDeviceAsync(config, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return NoContent();
        }
    }
}