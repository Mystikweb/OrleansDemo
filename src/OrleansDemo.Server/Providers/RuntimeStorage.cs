using Orleans.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;
using OrleansDemo.Models;
using System.Threading.Tasks;

namespace OrleansDemo.Server.Providers
{
    public class RuntimeStorage : IStorageProvider
    {
        private readonly RuntimeContext _context;

        public Logger Log { get; private set; }

        public string Name { get; private set; }

        public RuntimeStorage(RuntimeContext context) => _context = context;

        public Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            return Task.CompletedTask;
        }

        public Task Close()
        {
            return Task.CompletedTask;
        }

        public Task Init(string name, IProviderRuntime providerRuntime, IProviderConfiguration config)
        {
            Name = name;
            Log = providerRuntime.GetLogger(name);

            return Task.CompletedTask;
        }

        public Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            return Task.CompletedTask;
        }

        public Task WriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            return Task.CompletedTask;
        }
    }
}
