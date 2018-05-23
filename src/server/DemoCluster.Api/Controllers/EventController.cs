using System.Collections.Generic;
using System.Threading.Tasks;
using DemoCluster.DAL;
using DemoCluster.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoCluster.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class EventController : Controller
    {
        private readonly IConfigurationStorage storage;

        public EventController(IConfigurationStorage storage)
        {
            this.storage = storage;
        }

        [HttpGet]
        public async Task<IEnumerable<EventConfig>> Get(string sort, int index, string filter)
        {
            return await storage.GetEventListAsync(sort, index, filter);            
        }

        [HttpGet("{eventId}")]
        public async Task<IActionResult> Get(int eventId)
        {
            return Ok(await storage.GetEventAsync(eventId));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EventConfig model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EventConfig result = await storage.SaveEventAsync(model);

            return CreatedAtAction("Get", new { eventId = result.EventId }, result);
        }

        [HttpDelete("{eventId}")]
        public async Task<IActionResult> Delete(int eventId)
        {
            await storage.RemoveEventAsync(eventId);
            return Ok();
        }
    }
}