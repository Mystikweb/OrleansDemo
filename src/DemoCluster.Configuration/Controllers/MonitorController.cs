using System.Collections.Generic;
using System.Threading.Tasks;
using DemoCluster.DAL;
using DemoCluster.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoCluster.Configuration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class MonitorController : Controller
    {
        private readonly IConfigurationStorage configuration;

        public MonitorController(IConfigurationStorage configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<IEnumerable<MonitorConfig>> Get()
        {
            return await configuration.GetMonitorListAsync();
        }

        [HttpGet("{monitorId}")]
        public async Task<ActionResult> GetById(string monitorId)
        {
            return Ok(await configuration.GetMonitorByIdAsync(monitorId));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MonitorConfig config)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MonitorConfig monitor = await configuration.SaveMonitorAsync(config);

            return CreatedAtAction(nameof(GetById), new { monitorId = monitor.MonitorId }, monitor);
        }

        [HttpDelete("{monitorId}")]
        public async Task<ActionResult> Delete(string monitorId)
        {
            await configuration.RemoveMonitorAsync(monitorId);
            return Ok();
        }
    }
}