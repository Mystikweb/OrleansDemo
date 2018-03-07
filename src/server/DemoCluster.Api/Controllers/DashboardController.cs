using DemoCluster.DAL;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace DemoCluster.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DashboardController : Controller
    {
        private readonly IRuntimeStorage runtime;
        private readonly IGrainFactory grainClient;

        public DashboardController(IRuntimeStorage runtimeStorage, IGrainFactory grainFactory)
        {
            runtime = runtimeStorage;
            grainClient = grainFactory;
        }
    }
}