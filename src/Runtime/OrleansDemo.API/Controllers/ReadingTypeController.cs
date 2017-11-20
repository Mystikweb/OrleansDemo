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
    [Route("api/ReadingType")]
    public class ReadingTypeController : Controller
    {
        private readonly ConfigurationContext _context;

        public ReadingTypeController(ConfigurationContext context)
        {
            _context = context;
        }

        // GET: api/ReadingType
        [HttpGet]
        public async Task<IEnumerable<ReadingTypeViewModel>> GetReadingTypes()
        {
            return await (from t in _context.ReadingTypes
                          select new ReadingTypeViewModel
                          {
                              Id = t.Id,
                              Name = t.Name,
                              UOM = t.Uom,
                              DataType = t.DataType
                          }).ToListAsync();
        }

        // GET: api/ReadingType/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReadingType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var readingType = await (from t in _context.ReadingTypes
                                     where t.Id == id
                                     select new ReadingTypeViewModel
                                     {
                                         Id = t.Id,
                                         Name = t.Name,
                                         UOM = t.Uom,
                                         DataType = t.DataType
                                     }).FirstOrDefaultAsync();

            if (readingType == null)
            {
                return NotFound();
            }

            return Ok(readingType);
        }

        // PUT: api/ReadingType/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReadingType([FromRoute] int id, [FromBody] ReadingTypeViewModel readingType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != readingType.Id)
            {
                return BadRequest();
            }

            ReadingType model = await _context.ReadingTypes.FirstOrDefaultAsync(t => t.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            model.Name = readingType.Name;
            model.Uom = readingType.UOM;
            model.DataType = readingType.DataType;

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReadingTypeExists(id))
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

        // POST: api/ReadingType
        [HttpPost]
        public async Task<IActionResult> PostReadingType([FromBody] ReadingTypeViewModel readingType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ReadingType model = new ReadingType()
            {
                Name = readingType.Name,
                Uom = readingType.UOM,
                DataType = readingType.DataType
            };

            _context.ReadingTypes.Add(model);
            await _context.SaveChangesAsync();

            readingType.Id = model.Id;

            return CreatedAtAction("GetReadingType", new { id = readingType.Id }, readingType);
        }

        // DELETE: api/ReadingType/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReadingType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var readingType = await _context.ReadingTypes.SingleOrDefaultAsync(m => m.Id == id);
            if (readingType == null)
            {
                return NotFound();
            }

            ReadingTypeViewModel result = new ReadingTypeViewModel()
            {
                Id = readingType.Id,
                Name = readingType.Name,
                UOM = readingType.Uom,
                DataType = readingType.DataType
            };

            _context.ReadingTypes.Remove(readingType);
            await _context.SaveChangesAsync();

            return Ok(result);
        }

        private bool ReadingTypeExists(int id)
        {
            return _context.ReadingTypes.Any(e => e.Id == id);
        }
    }
}