using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces.States;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoCluster.GrainInterfaces
{
    public interface ISensorJournalGrain : IGrainWithIntegerKey
    {
        Task<SensorState> Initialize(DeviceSensorConfig config);
        Task PushState(SensorHistoryState state);
    }
}
