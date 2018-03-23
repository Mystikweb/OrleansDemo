using DemoCluster.DAL.States;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoCluster.GrainInterfaces
{
    public interface IDeviceHistoryJournal : IGrainWithGuidKey
    {
        Task<DeviceState> Initialize();
        Task PushState(DeviceHistoryState state);
    }
}
