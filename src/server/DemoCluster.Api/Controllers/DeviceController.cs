using DemoCluster.DAL;
using DemoCluster.DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoCluster.Api.Controllers
{
    [Route("api/[controller]")]    
    public class DeviceController : Controller
    {
        private readonly IConfigurationStorage configurationStorage;

        public DeviceController(IConfigurationStorage configStore)
        {
            configurationStorage = configStore;
        }

        [HttpGet]
        public async Task<IEnumerable<DeviceConfig>> Get()
        {
            return await configurationStorage.GetDeviceListAsync();
        }

    }
}