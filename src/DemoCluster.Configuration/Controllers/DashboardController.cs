using DemoCluster.DAL;
using DemoCluster.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoCluster.Configuration.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DashboardController : Controller
    {

        public DashboardController()
        {
        }

        // [HttpGet]
        // public async Task<IEnumerable<DeviceSummary>> Get()
        // {
        //     return await runtime.GetDashboardSummary();
        // }
    }
}