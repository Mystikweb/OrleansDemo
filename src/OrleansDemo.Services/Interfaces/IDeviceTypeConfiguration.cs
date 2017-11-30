using OrleansDemo.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrleansDemo.Services.Interfaces
{
    public interface IDeviceTypeConfiguration : IDisposable
    {
        Task<bool> DeviceTypeExistsAsync(int id);
        Task<IEnumerable<DeviceTypeViewModel>> GetListAsync();
        Task<DeviceTypeViewModel> GetAsync(int id);
        Task<DeviceTypeFileViewModel> GetFileAsync(int id);
        Task<DeviceTypeViewModel> SaveAsync(DeviceTypeViewModel deviceType);
        Task<DeviceTypeViewModel> SaveImageFileAsync(DeviceTypeFileViewModel fileViewModel);
        Task RemoveAsync(int id);
    }
}
