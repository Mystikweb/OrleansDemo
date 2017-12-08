using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrleansDemo.Patterns.Registry
{
    public interface IRegistryGrain<TRegisteredGrain> : IGrainWithGuidKey
        where TRegisteredGrain : IGrain
    {
        Task<TRegisteredGrain> RegisterGrain(TRegisteredGrain item);

        Task<List<TRegisteredGrain>> GetRegisteredGrains();

        Task<TRegisteredGrain> RemoveGrain(TRegisteredGrain item);
    }
}
