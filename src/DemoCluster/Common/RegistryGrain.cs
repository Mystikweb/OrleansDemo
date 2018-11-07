using DemoCluster.States;
using Orleans;
using Orleans.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoCluster
{
    [StorageProvider(ProviderName = "MemoryStorage")]
    public abstract class RegistryGrain<TRegisteredGrain> : Grain<RegistryState<TRegisteredGrain>>,
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

        public async Task<TRegisteredGrain> UnregisterGrain(TRegisteredGrain item)
        {
            if (State.RegisteredGrains.Contains(item))
            {
                State.RegisteredGrains.Remove(item);
                await WriteStateAsync();
            }
            return item;
        }
    }
}