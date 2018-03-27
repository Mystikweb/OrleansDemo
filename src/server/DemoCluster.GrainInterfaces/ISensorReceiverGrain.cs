using DemoCluster.GrainInterfaces.States;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoCluster.GrainInterfaces
{
    public interface ISensorReceiverGrain : IGrainWithIntegerKey
    {
        Task Initialize(SensorReceiverState state);
        Task<bool> IsReceiving();
        Task<bool> StartReceiver();
        Task<bool> StopReceiver();
    }
}
