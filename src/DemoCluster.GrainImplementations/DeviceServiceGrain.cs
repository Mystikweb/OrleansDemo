using DemoCluster.GrainInterfaces;
using DemoCluster.Models;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Concurrency;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoCluster.GrainImplementations
{
    [StatelessWorker]
    public class DeviceServiceGrain :
        Grain,
        IDeviceServiceGrain
    {
        private readonly ILogger logger;
        private readonly IDeviceServiceClient deviceService;

        public DeviceServiceGrain(ILogger<DeviceServiceGrain> logger,
            IDeviceServiceClient deviceService)
        {
            this.logger = logger;
            this.deviceService = deviceService;
        }

        public Task<List<DeviceSummaryViewModel>> GetDevices()
        {
            logger.LogInformation($"Getting device list");
            return deviceService.GetDevices();
        }

        public Task<DeviceSummaryViewModel> AddDevice(DeviceViewModel device)
        {
            logger.LogInformation($"Adding device to silo");
            return deviceService.AddDevice(device);
        }

        public Task<DeviceSummaryViewModel> UpdateDevice(DeviceViewModel device)
        {
            logger.LogInformation($"Updating device");
            return deviceService.UpdateDevice(device);
        }

        public Task RemoveDevice(DeviceViewModel device)
        {
            logger.LogInformation($"Removing device");
            return deviceService.RemoveDevice(device);
        }
    }
}
