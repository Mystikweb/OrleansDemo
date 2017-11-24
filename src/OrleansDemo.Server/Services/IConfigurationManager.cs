using OrleansDemo.Models.Transfer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrleansDemo.Server.Services
{
    public interface IConfigurationManager : IDisposable
    {
        Task<List<DeviceConfiguration>> GetDeviceConfigurations();
    }
}
