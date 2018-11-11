using DemoCluster.GrainInterfaces;
using DemoCluster.Models;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Concurrency;
using Orleans.Core;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Text;
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

        public override Task Start()
        {
            
            return base.Start();
        }

        public Task<IDeviceGrain> AddDevice(DeviceViewModel device)
        {
            throw new NotImplementedException();
        }

        public Task<IDeviceGrain> UpdateDevice(DeviceViewModel device)
        {
            throw new NotImplementedException();
        }

        public Task RemoveDevice(DeviceViewModel device)
        {
            throw new NotImplementedException();
        }
    }
}
