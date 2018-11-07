using System;
using System.Collections.Generic;

namespace DemoCluster.States
{
    [Serializable]
    public class RegistryState<TRegisteredGrain>
    {
        public HashSet<TRegisteredGrain> RegisteredGrains { get; set; } = new HashSet<TRegisteredGrain>();
    }
}
