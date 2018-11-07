using Microsoft.AspNetCore.Mvc;

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