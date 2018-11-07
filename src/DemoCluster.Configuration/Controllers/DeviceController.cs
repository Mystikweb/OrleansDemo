using DemoCluster.DAL.Logic;
using DemoCluster.Models;
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
        public async Task<IEnumerable<DeviceViewModel>> Get()
        {
            return await deviceLogic.GetDeviceListAsync();
        }

        [HttpGet("{deviceId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<DeviceViewModel>> GetById(string deviceId)
        {
            DeviceViewModel result = await deviceLogic.GetDeviceAsync(Guid.Parse(deviceId));
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("name/{name}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<DeviceViewModel>> GetByName(string name)
        {
            DeviceViewModel result = await deviceLogic.GetDeviceAsync(name, HttpContext.RequestAborted);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        public async Task<ActionResult<DeviceViewModel>> Post([FromBody] DeviceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            DeviceViewModel result = null;

            try
            {
                result = await deviceLogic.CreateDeviceAsync(model);
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
        public async Task<ActionResult> Put(string deviceId, [FromBody] DeviceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DeviceViewModel device = await deviceLogic.GetDeviceAsync(Guid.Parse(deviceId));
            if (device == null)
            {
                return NotFound();
            }

            try
            {
                await deviceLogic.UpdateDeviceAsync(model);
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
            DeviceViewModel model = await deviceLogic.GetDeviceAsync(Guid.Parse(deviceId));
            if (model == null)
            {
                return NotFound();
            }

            try
            {
                await deviceLogic.DeleteDeviceAsync(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return NoContent();
        }
    }
}