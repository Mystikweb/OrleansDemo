using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoCluster.DAL.States;
using DemoCluster.GrainInterfaces.Patterns;
using Orleans;
using Orleans.Providers;

namespace DemoCluster.GrainImplementations.Patterms
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
    }
}