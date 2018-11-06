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
        [HttpGet]
        public Task<List<MonitorConfig>> Get()
        {
            return Task.FromResult(new List<MonitorConfig>());
        }
    }
}