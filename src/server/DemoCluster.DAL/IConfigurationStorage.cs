using DemoCluster.DAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoCluster.DAL
{
    public interface IConfigurationStorage
    {
        Task<List<DeviceConfig>> GetDeviceListAsync();
        Task<DeviceConfig> GetDeviceAsync(Guid deviceId);
        Task<DeviceConfig> SaveDeviceAsync(DeviceConfig device);
    }
}