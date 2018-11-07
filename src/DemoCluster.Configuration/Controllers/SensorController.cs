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
    public class SensorController : Controller
    {
        private readonly SensorLogic sensorLogic;

        public SensorController(SensorLogic sensorLogic)
        {
            this.sensorLogic = sensorLogic;
        }

        [HttpGet]
        public async Task<IEnumerable<SensorViewModel>> Get()
        {
            return await sensorLogic.GetSensorListAsync();
        }

        [HttpGet("{sensorId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<SensorViewModel>> Get(int sensorId)
        {
            SensorViewModel result = await sensorLogic.GetSensorAsync(sensorId);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        public async Task<ActionResult<SensorViewModel>> Post([FromBody] SensorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SensorViewModel result = null;

            try
            {
                result = await sensorLogic.SaveSensorAsync(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return CreatedAtAction(nameof(Get), new { sensorId = result.SensorId }, result);
        }

        [HttpPut("{sensorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> Put(int sensorId, [FromBody] SensorViewModel model)
        {

            SensorViewModel sensor = await sensorLogic.GetSensorAsync(sensorId);
            if (sensor == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await sensorLogic.SaveSensorAsync(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return NoContent();
        }

        [HttpDelete("{sensorId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> Delete(int sensorId)
        {
            SensorViewModel model = await sensorLogic.GetSensorAsync(sensorId);
            if (model == null)
            {
                return NotFound();
            }

            try
            {
                await sensorLogic.RemoveSensorAsync(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return NoContent();
        }
    }
}