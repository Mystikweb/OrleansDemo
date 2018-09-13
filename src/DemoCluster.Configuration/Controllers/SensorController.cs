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
    public class SensorController : Controller
    {
        private readonly SensorLogic sensorLogic;

        public SensorController(SensorLogic sensorLogic)
        {
            this.sensorLogic = sensorLogic;
        }

        [HttpGet]
        public async Task<IEnumerable<SensorConfig>> Get()
        {
            return await sensorLogic.GetSensorListAsync(HttpContext.RequestAborted);
        }

        [HttpGet("{sensorId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<SensorConfig>> Get(int sensorId)
        {
            SensorConfig result = await sensorLogic.GetSensorAsync(sensorId, HttpContext.RequestAborted);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        public async Task<ActionResult<SensorConfig>> Post([FromBody] SensorConfig config)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SensorConfig result = null;

            try
            {
                result = await sensorLogic.SaveSensorAsync(config, HttpContext.RequestAborted);
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
        public async Task<ActionResult> Put(int sensorId, [FromBody] SensorConfig config)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SensorConfig sensor = await sensorLogic.GetSensorAsync(sensorId, HttpContext.RequestAborted);
            if (sensor == null)
            {
                return NotFound();
            }

            try
            {
                await sensorLogic.SaveSensorAsync(config, HttpContext.RequestAborted);
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
            SensorConfig config = await sensorLogic.GetSensorAsync(sensorId, HttpContext.RequestAborted);
            if (config == null)
            {
                return NotFound();
            }

            try
            {
                await sensorLogic.RemoveSensorAsync(config, HttpContext.RequestAborted);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return NoContent();
        }
    }
}