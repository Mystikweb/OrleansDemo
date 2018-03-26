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
        Task<SensorState> Initialize(SensorState initialState);
        Task PushState(SensorHistoryState state);
    }
}
