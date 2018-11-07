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
    public class StateController : Controller
    {
        private readonly StateLogic stateLogic;

        public StateController(StateLogic stateLogic)
        {
            this.stateLogic = stateLogic;
        }

        [HttpGet]
        public async Task<IEnumerable<StateViewModel>> Get()
        {
            return await stateLogic.GetStateListAsync();
        }

        [HttpGet("{stateId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<StateViewModel>> Get(int stateId)
        {
            StateViewModel result = await stateLogic.GetStateAsync(stateId);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        public async Task<ActionResult<StateViewModel>> Post([FromBody] StateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StateViewModel result = null;

            try
            {
                result = await stateLogic.SaveStateAsync(model);
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
        public async Task<ActionResult> Put(int stateId, [FromBody] StateViewModel model)
        {
            StateViewModel StateViewModel = await stateLogic.GetStateAsync(stateId);
            if (StateViewModel == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await stateLogic.SaveStateAsync(model);
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
            StateViewModel model = await stateLogic.GetStateAsync(stateId);
            if (model == null)
            {
                return NotFound();
            }

            try
            {
                await stateLogic.RemoveStateAsync(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return NoContent();
        }
    }
}