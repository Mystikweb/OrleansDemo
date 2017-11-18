﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrleansDemo.API.Models;

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
        public async Task<IEnumerable<DeviceType>> GetDeviceTypes(string sort, string order, int page)
        {
            string sortOptions = $"{sort}_{order}";
            var deviceTypes = from t in _context.DeviceTypes select t;

            switch (sortOptions)
            {
                case "name_desc":
                    deviceTypes = deviceTypes.OrderByDescending(t => t.Name);
                    break;
                default:
                    deviceTypes = deviceTypes.OrderBy(t => t.Name);
                    break;
            }

            return await deviceTypes.ToListAsync();
        }

        // GET: api/DeviceType/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeviceType([FromRoute] int id)
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

            return Ok(deviceType);
        }

        // PUT: api/DeviceType/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeviceType([FromRoute] int id, [FromBody] DeviceType deviceType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != deviceType.Id)
            {
                return BadRequest();
            }

            _context.Entry(deviceType).State = EntityState.Modified;

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
        public async Task<IActionResult> PostDeviceType([FromBody] DeviceType deviceType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.DeviceTypes.Add(deviceType);
            await _context.SaveChangesAsync();

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

            _context.DeviceTypes.Remove(deviceType);
            await _context.SaveChangesAsync();

            return Ok(deviceType);
        }

        private bool DeviceTypeExists(int id)
        {
            return _context.DeviceTypes.Any(e => e.Id == id);
        }
    }
}