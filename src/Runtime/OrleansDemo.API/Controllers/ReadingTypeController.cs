using Microsoft.AspNetCore.Mvc;
using OrleansDemo.Models.ViewModels.Configuration;
using OrleansDemo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrleansDemo.API.Controllers
{
    [Produces("application/json")]
    [Route("api/ReadingType")]
    public class ReadingTypeController : Controller
    {
        private readonly IReadingTypeConfiguration readingType;

        public ReadingTypeController(IReadingTypeConfiguration readingTypeConfiguration)
        {
            readingType = readingTypeConfiguration;
        }

        // GET: api/ReadingType
        [HttpGet]
        public async Task<IEnumerable<ReadingTypeViewModel>> GetReadingTypes()
        {
            return await readingType.GetListAsync();
        }

        // GET: api/ReadingType/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReadingType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool exists = await readingType.ReadingTypeExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            return Ok(await readingType.GetAsync(id));
        }

        // PUT: api/ReadingType/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReadingType([FromRoute] int id, [FromBody] ReadingTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool exists = await readingType.ReadingTypeExistsAsync(id);
            if (id != model.Id || !exists)
            {
                return BadRequest();
            }

            try
            {
                await readingType.SaveAsync(model);
            }
            catch (Exception)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/ReadingType
        [HttpPost]
        public async Task<IActionResult> PostReadingType([FromBody] ReadingTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ReadingTypeViewModel result = await readingType.SaveAsync(model);

            return CreatedAtAction("GetReadingType", new { id = result.Id }, result);
        }

        // DELETE: api/ReadingType/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReadingType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool exists = await readingType.ReadingTypeExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            ReadingTypeViewModel result = await readingType.GetAsync(id);

            await readingType.RemoveAsync(id);

            return Ok(result);
        }
    }
}