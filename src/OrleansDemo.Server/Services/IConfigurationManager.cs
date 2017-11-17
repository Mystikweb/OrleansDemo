using OrleansDemo.Models.Transfer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrleansDemo.Server.Services
{
    public interface IConfigurationManager
    {
        Task<List<DeviceConfiguration>> GetDeviceConfigurations();
    }
}
