using DemoCluster.DAL;
using DemoCluster.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoCluster.Configuration.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class RuntimeController : Controller
    {
        private readonly ILogger logger;

        public RuntimeController(ILogger<RuntimeController> logger)
        {
            this.logger = logger;
        }
    }
}
