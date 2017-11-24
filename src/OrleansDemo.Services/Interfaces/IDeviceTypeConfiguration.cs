using OrleansDemo.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrleansDemo.Services.Interfaces
{
    public interface IDeviceTypeConfiguration : IDisposable
    {
        Task<bool> DeviceTypeExists(int id);
        Task<IEnumerable<DeviceTypeViewModel>> GetList();
        Task<DeviceTypeViewModel> Get(int id);
        Task<DeviceTypeViewModel> Save(DeviceTypeViewModel deviceType);
        Task Remove(int id);
    }
}
