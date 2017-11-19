using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrleansDemo.API.Models;
using OrleansDemo.API.ViewModels;

namespace OrleansDemo.API.Controllers
{
    [Produces("application/json")]
    [Route("api/DeviceType")]
    public class DeviceTypeController : Controller
    {
        private readonly ConfigurationContext _context;

        public DeviceTypeController(ConfigurationContext context)
        {
            _context = context;
        }

        // GET: api/DeviceType
        [HttpGet]
        public async Task<IEnumerable<DeviceTypeViewModel>> GetDeviceTypes()
        {
            return await (from t in _context.DeviceTypes
                          select new DeviceTypeViewModel
                          {
                              Id = t.Id,
                              Name = t.Name,
                              Active = t.Active.HasValue ? t.Active.Value : false
                          }).ToListAsync();
        }

        // GET: api/DeviceType/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeviceType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deviceType = await (from t in _context.DeviceTypes
                                    where t.Id == id
                                    select new DeviceTypeViewModel
                                    {
                                        Id = t.Id,
                                        Name = t.Name,
                                        Active = t.Active.HasValue ? t.Active.Value : false
                                    }).FirstOrDefaultAsync();

            if (deviceType == null)
            {
                return NotFound();
            }

            return Ok(deviceType);
        }

        // PUT: api/DeviceType/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeviceType([FromRoute] int id, [FromBody] DeviceTypeViewModel deviceType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != deviceType.Id)
            {
                return BadRequest();
            }

            DeviceType model = await _context.DeviceTypes.FirstOrDefaultAsync(t => t.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            model.Name = deviceType.Name;
            model.Active = deviceType.Active;

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DeviceType
        [HttpPost]
        public async Task<IActionResult> PostDeviceType([FromBody] DeviceTypeViewModel deviceType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DeviceType model = new DeviceType()
            {
                Name = deviceType.Name,
                Active = deviceType.Active
            };

            _context.DeviceTypes.Add(model);
            await _context.SaveChangesAsync();

            deviceType.Id = model.Id;

            return CreatedAtAction("GetDeviceType", new { id = deviceType.Id }, deviceType);
        }

        // DELETE: api/DeviceType/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeviceType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deviceType = await _context.DeviceTypes.SingleOrDefaultAsync(m => m.Id == id);
            if (deviceType == null)
            {
                return NotFound();
            }

            DeviceTypeViewModel result = new DeviceTypeViewModel()
            {
                Id = deviceType.Id,
                Name = deviceType.Name,
                Active = deviceType.Active.Value
            };

            _context.DeviceTypes.Remove(deviceType);
            await _context.SaveChangesAsync();

            return Ok(result);
        }

        private bool DeviceTypeExists(int id)
        {
            return _context.DeviceTypes.Any(e => e.Id == id);
        }
    }
}