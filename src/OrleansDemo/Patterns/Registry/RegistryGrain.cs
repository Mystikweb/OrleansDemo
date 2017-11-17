using Orleans;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrleansDemo.Patterns.Registry
{
    [StorageProvider(ProviderName = "OrleansDemoStorage")]
    public abstract class RegistryGrain<TRegisteredGrain> : Grain<RegistryGrainState<TRegisteredGrain>>, 
        IRegistryGrain<TRegisteredGrain> where TRegisteredGrain : IGrain
    {
        public Task<List<TRegisteredGrain>> GetRegisteredGrains()
        {
            return Task.FromResult(State.RegisteredGrains.ToList());
        }

        public async Task<TRegisteredGrain> RegisterGrain(TRegisteredGrain item)
        {
            if (State.RegisteredGrains == null)
            {
                State.RegisteredGrains = new HashSet<TRegisteredGrain>();
            }
            State.RegisteredGrains.Add(item);
            await WriteStateAsync();
            return item;
        }
    }
}
