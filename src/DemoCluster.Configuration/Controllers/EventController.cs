using DemoCluster.DAL.Logic;
using DemoCluster.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoCluster.Configuration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class EventController : Controller
    {
        private readonly EventTypeLogic eventLogic;

        public EventController(EventTypeLogic eventLogic)
        {
            this.eventLogic = eventLogic;
        }

        [HttpGet]
        public async Task<IEnumerable<EventTypeViewModel>> Get()
        {
            return await eventLogic.GetEventListAsync();
        }

        [HttpGet("{eventId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<EventTypeViewModel>> Get(int eventId)
        {
            EventTypeViewModel result = await eventLogic.GetEventAsync(eventId);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        public async Task<ActionResult<EventTypeViewModel>> Post([FromBody] EventTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EventTypeViewModel result = null;

            try
            {
                result = await eventLogic.SaveEventAsync(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return CreatedAtAction(nameof(Get), new { eventId = result.EventId }, result);
        }

        [HttpPut("{eventId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> Put(int eventId, [FromBody] EventTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EventTypeViewModel eventType = await eventLogic.GetEventAsync(eventId);
            if (eventType == null)
            {
                return NotFound();
            }

            try
            {
                await eventLogic.SaveEventAsync(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return NoContent();
        }

        [HttpDelete("{eventId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> Delete(int eventId)
        {
            EventTypeViewModel model = await eventLogic.GetEventAsync(eventId);
            if (model == null)
            {
                return NotFound();
            }

            try
            {
                await eventLogic.RemoveEventAsync(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return NoContent();
        }
    }
}