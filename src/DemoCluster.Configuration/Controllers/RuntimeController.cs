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
        private readonly IRuntimeStorage runtime;
        private readonly ILogger logger;

        public RuntimeController(IRuntimeStorage runtime, 
            ILogger<RuntimeController> logger)
        {
            this.runtime = runtime;
            this.logger = logger;
        }
    }
}
