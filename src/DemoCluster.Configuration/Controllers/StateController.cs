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
    public class StateController : Controller
    {
        private readonly StateLogic stateLogic;

        public StateController(StateLogic stateLogic)
        {
            this.stateLogic = stateLogic;
        }

        [HttpGet]
        public async Task<IEnumerable<StateConfig>> Get()
        {
            return await stateLogic.GetStateListAsync(HttpContext.RequestAborted);
        }

        [HttpGet("{stateId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<StateConfig>> Get(int stateId)
        {
            StateConfig result = await stateLogic.GetStateAsync(stateId, HttpContext.RequestAborted);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        public async Task<ActionResult<StateConfig>> Post([FromBody] StateConfig config)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StateConfig result = null;

            try
            {
                result = await stateLogic.SaveStateAsync(config, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return CreatedAtAction(nameof(Get), new { stateId = result.StateId }, result);
        }

        [HttpPut("{stateId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> Put(int stateId, [FromBody] StateConfig config)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StateConfig stateConfig = await stateLogic.GetStateAsync(stateId, HttpContext.RequestAborted);
            if (stateConfig == null)
            {
                return NotFound();
            }

            try
            {
                await stateLogic.SaveStateAsync(config, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return NoContent();
        }

        [HttpDelete("{stateId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> Delete(int stateId)
        {
            StateConfig config = await stateLogic.GetStateAsync(stateId, HttpContext.RequestAborted);
            if (config == null)
            {
                return NotFound();
            }

            try
            {
                await stateLogic.RemoveStateAsync(config, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return NoContent();
        }
    }
}