using Orleans;
using OrleansDemo.Models.Transfer;
using OrleansDemo.Models.ViewModels.Runtime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrleansDemo.Interfaces
{
    public interface IDeviceGrain : IGrainWithGuidKey
    {
        Task<Guid> GetDeviceId();
        Task Initialize(DeviceConfiguration configuration);
        Task Start();
        Task Stop();
        Task RecordValue(string value);
    }
}
