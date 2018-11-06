using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoCluster.Messaging.Hubs
{
    public class SensorValueHub : Hub
    {
        private readonly ILogger logger;
        private readonly IClusterClient clusterClient;

        public SensorValueHub(ILogger<SensorValueHub> logger,
            IClusterClient clusterClient)
        {
            this.logger = logger;
            this.clusterClient = clusterClient;
        }
    }
}
