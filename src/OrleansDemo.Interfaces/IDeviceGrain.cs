using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrleansDemo.Interfaces
{
    public interface IDeviceGrain : IGrainWithGuidKey
    {
        Task Start();
        Task Stop();
    }
}
