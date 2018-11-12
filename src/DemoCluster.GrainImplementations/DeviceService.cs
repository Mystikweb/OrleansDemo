using DemoCluster.GrainInterfaces;
using DemoCluster.Models;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Concurrency;
using Orleans.Core;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoCluster.GrainImplementations
{
    [Reentrant]
    public class DeviceService :
        GrainService,
        IDeviceService
    {
        private readonly ILogger logger;
        private readonly IGrainFactory grainFactory;
        private readonly HashSet<DeviceViewModel> devices;

        private bool hasStarted = false;

        public DeviceService(IGrainIdentity grainIdentity, 
            Silo silo, 
            ILoggerFactory loggerFactory, 
            IGrainFactory grainFactory)
            : base(grainIdentity, silo, loggerFactory)
        {
            this.grainFactory = grainFactory;

            logger = loggerFactory.CreateLogger<DeviceService>();
            devices = new HashSet<DeviceViewModel>();
        }

        protected async override Task StartInBackground()
        {
            IDeviceRegistry registry = grainFactory.GetGrain<IDeviceRegistry>(0);
            List<IDeviceGrain> deviceGrains = await registry.GetRegisteredGrains();

            foreach (IDeviceGrain deviceGrain in deviceGrains)
            {
                grainFactory.BindGrainReference(deviceGrain);
                DeviceViewModel deviceModel = await deviceGrain.GetDeviceModel();
                
                if (!devices.Contains(deviceModel))
                {
                    devices.Add(deviceModel);
                }
            }

            hasStarted = true;

            await base.StartInBackground();
        }

        public async Task<List<DeviceSummaryViewModel>> GetDevices()
        {
            if (hasStarted)
            {
                IDeviceRegistry deviceRegistry = grainFactory.GetGrain<IDeviceRegistry>(0);
                List<IDeviceGrain> deviceGrains = await deviceRegistry.GetRegisteredGrains();

                List<DeviceSummaryViewModel> results = new List<DeviceSummaryViewModel>();

                foreach (IDeviceGrain device in deviceGrains)
                {
                    grainFactory.BindGrainReference(device);
                    results.Add(await device.GetDeviceSummary());
                }

                return results;
            }

            return null;
        }

        public async Task<DeviceSummaryViewModel> AddDevice(DeviceViewModel device)
        {
            IDeviceGrain deviceGrain = grainFactory.GetGrain<IDeviceGrain>(Guid.Parse(device.DeviceId));
            IDeviceRegistry registry = grainFactory.GetGrain<IDeviceRegistry>(0);

            if (!devices.Contains(device))
            {
                await deviceGrain.Start(device);
                await registry.RegisterGrain(deviceGrain);
                devices.Add(device);
            }

            return await deviceGrain.GetDeviceSummary();
        }

        public async Task<DeviceSummaryViewModel> UpdateDevice(DeviceViewModel device)
        {
            IDeviceGrain deviceGrain = grainFactory.GetGrain<IDeviceGrain>(Guid.Parse(device.DeviceId));
            await deviceGrain.UpdateDevice(device);

            return await deviceGrain.GetDeviceSummary();
        }

        public async Task RemoveDevice(DeviceViewModel device)
        {
            if (devices.Contains(device))
            {
                IDeviceGrain deviceGrain = grainFactory.GetGrain<IDeviceGrain>(Guid.Parse(device.DeviceId));
                await deviceGrain.UpdateDevice(device, false);

                IDeviceRegistry registry = grainFactory.GetGrain<IDeviceRegistry>(0);
                await registry.UnregisterGrain(deviceGrain);

                devices.Remove(device);
            }
        }
    }
}
