using DemoCluster.GrainInterfaces;
using DemoCluster.Models;
using Orleans.Runtime.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoCluster.GrainImplementations
{
    public class DeviceServiceClient :
        GrainServiceClient<IDeviceService>,
        IDeviceServiceClient
    {
        public DeviceServiceClient(IServiceProvider serviceProvider)
            : base(serviceProvider) { }

        public Task<List<DeviceSummaryViewModel>> GetDevices() => GrainService.GetDevices();

        public Task<DeviceSummaryViewModel> AddDevice(DeviceViewModel device) => GrainService.AddDevice(device);

        public Task<DeviceSummaryViewModel> UpdateDevice(DeviceViewModel device) => GrainService.UpdateDevice(device);

        public Task RemoveDevice(DeviceViewModel device) => GrainService.RemoveDevice(device);
    }
}
