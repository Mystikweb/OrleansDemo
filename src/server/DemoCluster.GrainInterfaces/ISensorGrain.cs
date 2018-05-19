using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces.States;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoCluster.GrainInterfaces
{
    public interface ISensorGrain : IGrainWithIntegerKey
    {
        Task Initialize(DeviceSensorConfig config);
        Task<SensorState> GetState();
        Task StartReceiving();
        Task StopReceiving();
    }
}
