using Microsoft.AspNetCore.Mvc;
using OrleansDemo.Models.ViewModels.Configuration;
using OrleansDemo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrleansDemo.API.Controllers
{
    [Produces("application/json")]
    [Route("api/DeviceType")]
    public class DeviceTypeController : Controller
    {
        private readonly IDeviceTypeConfiguration deviceType;

        public DeviceTypeController(IDeviceTypeConfiguration deviceTypeConfiguration)
        {
            deviceType = deviceTypeConfiguration;
        }

        // GET: api/DeviceType
        [HttpGet]
        public async Task<IEnumerable<DeviceTypeViewModel>> GetDeviceTypes()
        {
            return await deviceType.GetListAsync();
        }

        // GET: api/DeviceType/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeviceType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool exists = await deviceType.DeviceTypeExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            return Ok(await deviceType.GetAsync(id));
        }

        // PUT: api/DeviceType/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeviceType([FromRoute] int id, [FromBody] DeviceTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool exists = await deviceType.DeviceTypeExistsAsync(id);
            if (id != model.Id || !exists)
            {
                return BadRequest();
            }

            try
            {
                await deviceType.SaveAsync(model);
            }
            catch (Exception)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/DeviceType
        [HttpPost]
        public async Task<IActionResult> PostDeviceType([FromBody] DeviceTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DeviceTypeViewModel result;
            try
            {
                result = await deviceType.SaveAsync(model);
            }
            catch (Exception)
            {
                throw;
            }

            return CreatedAtAction("GetDeviceType", new { id = result.Id }, result);
        }

        // DELETE: api/DeviceType/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeviceType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool exists = await deviceType.DeviceTypeExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            DeviceTypeViewModel result = await deviceType.GetAsync(id);

            await deviceType.RemoveAsync(id);

            return Ok(result);
        }
    }
}