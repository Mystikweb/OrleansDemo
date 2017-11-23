﻿using System;
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
    [Route("api/Device")]
    public class DeviceController : Controller
    {
        private readonly ConfigurationContext _context;

        public DeviceController(ConfigurationContext context)
        {
            _context = context;
        }

        // GET: api/Device
        [HttpGet]
        public async Task<IEnumerable<DeviceViewModel>> GetDevices()
        {
            return await _context.Devices.Select(d => new DeviceViewModel
            {
                Id = d.Id,
                Name = d.Name,
                DeviceType = d.DeviceType.Name,
                Enabled = d.Enabled.HasValue ? d.Enabled.Value : false,
                RunOnStartup = d.RunOnStartup.HasValue ? d.RunOnStartup.Value : false,
                CreatedAt = d.CreatedAt,
                CreatedBy = d.CreatedBy,
                UpdatedAt = d.UpdatedAt,
                UpdatedBy = d.UpdatedBy,
                Readings = d.Readings.Select(r => new ReadingViewModel
                {
                    Id = r.Id,
                    ReadingTypeId = r.ReadingType.Id,
                    ReadyingType = r.ReadingType.Name,
                    ReadingUom = r.ReadingType.Uom,
                    ReadingDataType = r.ReadingType.DataType,
                    UpdatedAt = r.UpdatedAt,
                    UpdatedBy = r.UpdatedBy
                }).ToList()
            }).ToListAsync();
        }

        // GET: api/Device/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDevice([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var device = await _context.Devices.SingleOrDefaultAsync(m => m.Id == id);

            if (device == null)
            {
                return NotFound();
            }

            return Ok(device);
        }

        // PUT: api/Device/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDevice([FromRoute] Guid id, [FromBody] Device device)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != device.Id)
            {
                return BadRequest();
            }

            _context.Entry(device).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(id))
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

        // POST: api/Device
        [HttpPost]
        public async Task<IActionResult> PostDevice([FromBody] Device device)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Devices.Add(device);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDevice", new { id = device.Id }, device);
        }

        // DELETE: api/Device/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var device = await _context.Devices.SingleOrDefaultAsync(m => m.Id == id);
            if (device == null)
            {
                return NotFound();
            }

            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();

            return Ok(device);
        }

        private bool DeviceExists(Guid id)
        {
            return _context.Devices.Any(e => e.Id == id);
        }
    }
}