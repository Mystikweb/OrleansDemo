using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrleansDemo.Models.ViewModels.Configuration;
using OrleansDemo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrleansDemo.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Device")]
    public class DeviceController : Controller
    {
        private readonly IDeviceConfiguration device;

        public DeviceController(IDeviceConfiguration deviceConfiguration)
        {
            device = deviceConfiguration;
        }

        // GET: api/Device
        [HttpGet]
        public async Task<IEnumerable<DeviceViewModel>> GetDevices()
        {
            return await device.GetListAsync();
        }

        // GET: api/Device/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDevice([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool exists = await device.DeviceExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            return Ok(await device.GetAsync(id));
        }

        // PUT: api/Device/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDevice([FromRoute] Guid id, [FromBody] DeviceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != model.Id)
            {
                return BadRequest();
            }

            bool exists = await device.DeviceExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            try
            {
                await device.SaveAsync(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/Device
        [HttpPost]
        public async Task<IActionResult> PostDevice([FromBody] DeviceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DeviceViewModel result = await device.SaveAsync(model);

            return CreatedAtAction("GetDevice", new { id = result.Id }, result);
        }

        // DELETE: api/Device/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool exists = await device.DeviceExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            DeviceViewModel result = await device.GetAsync(id);

            await device.RemoveAsync(id);

            return Ok(result);
        }
    }
}