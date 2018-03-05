using DemoCluster.DAL;
using Microsoft.AspNetCore.Mvc;

namespace DemoCluster.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DashboardController : Controller
    {
        private readonly IRuntimeStorage runtime;

        public DashboardController(IRuntimeStorage runtimeStorage)
        {
            runtime = runtimeStorage;
        }
    }
}