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
    [Route("api/Reading")]
    public class ReadingController : Controller
    {
        private readonly ConfigurationContext _context;

        public ReadingController(ConfigurationContext context)
        {
            _context = context;
        }

        // GET: api/Reading
        [HttpGet]
        public IEnumerable<Reading> GetReadings()
        {
            return _context.Readings;
        }

        // GET: api/Reading/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReading([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reading = await _context.Readings.SingleOrDefaultAsync(m => m.Id == id);

            if (reading == null)
            {
                return NotFound();
            }

            return Ok(reading);
        }

        // PUT: api/Reading/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReading([FromRoute] Guid id, [FromBody] Reading reading)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reading.Id)
            {
                return BadRequest();
            }

            _context.Entry(reading).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReadingExists(id))
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

        // POST: api/Reading
        [HttpPost]
        public async Task<IActionResult> PostReading([FromBody] Reading reading)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Readings.Add(reading);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReading", new { id = reading.Id }, reading);
        }

        // DELETE: api/Reading/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReading([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reading = await _context.Readings.SingleOrDefaultAsync(m => m.Id == id);
            if (reading == null)
            {
                return NotFound();
            }

            _context.Readings.Remove(reading);
            await _context.SaveChangesAsync();

            return Ok(reading);
        }

        private bool ReadingExists(Guid id)
        {
            return _context.Readings.Any(e => e.Id == id);
        }
    }
}