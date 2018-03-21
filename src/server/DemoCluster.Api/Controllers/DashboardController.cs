using DemoCluster.DAL;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoCluster.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DashboardController : Controller
    {
        private readonly IRuntimeStorage runtime;
        private readonly IConfigurationStorage configuration;
        private readonly IGrainFactory factory;

        public DashboardController(IRuntimeStorage runtime, IConfigurationStorage configuration, IGrainFactory factory)
        {
            this.runtime = runtime;
            this.configuration = configuration;
            this.factory = factory;
        }

        [HttpGet]
        public async Task<IEnumerable<DeviceSummary>> Get()
        {
            return await runtime.GetDashboardSummary();
        }
    }
}