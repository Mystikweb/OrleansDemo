using System;
using System.Collections.Generic;
using System.Text;

namespace OrleansDemo.Patterns.Registry
{
    [Serializable]
    public class RegistryGrainState<TRegisteredGrain>
    {
        public HashSet<TRegisteredGrain> RegisteredGrains { get; set; } = new HashSet<TRegisteredGrain>();
    }
}
