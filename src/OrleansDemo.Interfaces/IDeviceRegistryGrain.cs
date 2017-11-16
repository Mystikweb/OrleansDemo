using OrleansDemo.Patterns.Registry;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrleansDemo.Interfaces
{
    public interface IDeviceRegistryGrain : IRegistryGrain<IDeviceGrain>
    {
    }
}
