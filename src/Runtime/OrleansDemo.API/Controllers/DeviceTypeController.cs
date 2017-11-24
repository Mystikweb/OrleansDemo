using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrleansDemo.Models.ViewModels;
using OrleansDemo.Models.Configuration;

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
            return await (_context.DeviceTypes.Select(t => new DeviceTypeViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Active = t.Active.HasValue ? t.Active.Value : false,
                ReadingTypes = t.DeviceTypeReadingTypes.Select(r => new DeviceTypeReadingTypeViewModel
                {
                    Id = r.Id,
                    ReadingTypeId = r.ReadingTypeId,
                    ReadingType = r.ReadingType.Name,
                    Active = r.Active.HasValue ? r.Active.Value : false
                }).ToList()
            })).ToListAsync();
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
                                        Active = t.Active.HasValue ? t.Active.Value : false,
                                        ReadingTypes = t.DeviceTypeReadingTypes.Select(r => new DeviceTypeReadingTypeViewModel
                                        {
                                            Id = r.Id,
                                            ReadingTypeId = r.ReadingTypeId,
                                            ReadingType = r.ReadingType.Name,
                                            Active = r.Active.HasValue ? r.Active.Value : false
                                        }).ToList()
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

            if (id != deviceType.Id || !DeviceTypeExists(id))
            {
                return BadRequest();
            }

            DeviceType model = await _context.DeviceTypes.FirstOrDefaultAsync(t => t.Id == id);

            model.Name = deviceType.Name;
            model.Active = deviceType.Active;

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            foreach (var item in deviceType.ReadingTypes)
            {
                DeviceTypeReadingType assoc = null;
                if (item.Id.HasValue)
                {
                    assoc = await _context.DeviceTypeReadingTypes.FirstOrDefaultAsync(a => a.Id == item.Id.Value);
                    assoc.Active = item.Active;
                    _context.Entry(assoc).State = EntityState.Modified;
                }
                else
                {
                    assoc = new DeviceTypeReadingType();
                    assoc.DeviceTypeId = model.Id;
                    assoc.ReadingTypeId = item.ReadingTypeId;
                    assoc.Active = item.Active;
                    _context.DeviceTypeReadingTypes.Add(assoc);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
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

            foreach (var item in deviceType.ReadingTypes)
            {
                DeviceTypeReadingType deviceTypeReadingType = new DeviceTypeReadingType()
                {
                    DeviceTypeId = model.Id,
                    ReadingTypeId = item.ReadingTypeId,
                    Active = true
                };

                _context.DeviceTypeReadingTypes.Add(deviceTypeReadingType);
            }

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