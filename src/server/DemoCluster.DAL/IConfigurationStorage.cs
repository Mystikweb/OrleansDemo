using DemoCluster.DAL.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoCluster.DAL
{
    public interface IConfigurationStorage
    {
        Task<List<DeviceConfig>> GetDeviceListAsync();
    }
}