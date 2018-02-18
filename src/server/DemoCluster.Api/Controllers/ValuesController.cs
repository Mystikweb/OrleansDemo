using Microsoft.AspNetCore.Mvc;

namespace DemoCluster.Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "Hello World";
        }
    }
}