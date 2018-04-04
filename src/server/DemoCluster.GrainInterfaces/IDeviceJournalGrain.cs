using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces.States;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoCluster.GrainInterfaces
{
    public interface IDeviceJournalGrain : IGrainWithGuidKey
    {
        Task<DeviceState> Initialize(DeviceConfig config);
        Task PushState(DeviceHistoryState state);
    }
}
