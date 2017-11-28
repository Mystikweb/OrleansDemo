using OrleansDemo.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrleansDemo.Services.Interfaces
{
    public interface IDeviceConfiguration : IDisposable
    {
        Task<bool> DeviceExistsAsync(Guid id);
        Task<IEnumerable<DeviceViewModel>> GetListAsync();
        Task<DeviceViewModel> GetAsync(Guid id);
        Task<DeviceViewModel> SaveAsync(DeviceViewModel device);
        Task RemoveAsync(Guid id);
    }
}
