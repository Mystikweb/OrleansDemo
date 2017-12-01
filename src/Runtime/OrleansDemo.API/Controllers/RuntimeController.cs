using Microsoft.AspNetCore.Mvc;
using OrleansDemo.Models.Configuration;
using OrleansDemo.Models.Transfer;
using System.Collections.Generic;
using System.Linq;

namespace OrleansDemo.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Runtime")]
    public class RuntimeController : Controller
    {
        private readonly ConfigurationContext _context;

        public RuntimeController(ConfigurationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Configuration")]
        public IEnumerable<DeviceConfiguration> GetDeviceConfigurations()
        {
            return _context.Devices.Select(d => new DeviceConfiguration
            {
                DeviceId = d.Id,
                Name = d.Name,
                DeviceType = d.DeviceType.Name,
                IsEnabled = d.Enabled ?? false,
                ReadingConfigurations = d.Readings.Select(r => new DeviceReadingConfiguration
                {
                    ReadingType = r.ReadingType.Name,
                    UOM = r.ReadingType.Uom,
                    DataType = r.ReadingType.DataType
                }).ToList()
            });
        }

        [HttpGet]
        [Route("Configuration/{name}")]
        public DeviceConfiguration GetDeviceConfiguration(string name)
        {
            return _context.Devices.Select(d => new DeviceConfiguration
            {
                DeviceId = d.Id,
                Name = d.Name,
                DeviceType = d.DeviceType.Name,
                IsEnabled = d.Enabled ?? false,
                ReadingConfigurations = d.Readings.Select(r => new DeviceReadingConfiguration
                {
                    ReadingType = r.ReadingType.Name,
                    DataType = r.ReadingType.DataType,
                    UOM = r.ReadingType.Uom
                }).ToList()
            }).FirstOrDefault(d => d.Name == name);
        }
    }
}