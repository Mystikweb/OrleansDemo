using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DemoCluster.DAL;
using DemoCluster.DAL.Logic;
using DemoCluster.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoCluster.Configuration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class EventController : Controller
    {
        private readonly EventLogic eventLogic;

        public EventController(EventLogic eventLogic)
        {
            this.eventLogic = eventLogic;
        }

        [HttpGet]
        public async Task<IEnumerable<EventConfig>> Get()
        {
            return await eventLogic.GetEventListAsync(HttpContext.RequestAborted);
        }

        [HttpGet("{eventId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<EventConfig>> Get(int eventId)
        {
            EventConfig result = await eventLogic.GetEventAsync(eventId, HttpContext.RequestAborted);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        public async Task<ActionResult<EventConfig>> Post([FromBody] EventConfig config)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EventConfig result = null;

            try
            {
                result = await eventLogic.SaveEventAsync(config, HttpContext.RequestAborted);
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
        public async Task<ActionResult> Put(int eventId, [FromBody] EventConfig config)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EventConfig eventType = await eventLogic.GetEventAsync(eventId, HttpContext.RequestAborted);
            if (eventType == null)
            {
                return NotFound();
            }

            try
            {
                await eventLogic.SaveEventAsync(config, HttpContext.RequestAborted);
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
            EventConfig config = await eventLogic.GetEventAsync(eventId, HttpContext.RequestAborted);
            if (config == null)
            {
                return NotFound();
            }

            try
            {
                await eventLogic.RemoveEventAsync(config, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return NoContent();
        }
    }
}