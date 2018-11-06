using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces;
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

        public async Task RegisterSensor(int deviceSensorId)
        {
            ISensorGrain sensorGrain = clusterClient.GetGrain<ISensorGrain>(deviceSensorId);
            SensorStateSummary state = await sensorGrain.GetDeviceState();
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Sensor_{deviceSensorId}");

            await Clients.Group($"Sensor_{deviceSensorId}").SendAsync("CurrentState", state);
        }

        public async Task Recordvalue(SensorValueItem item)
        {
            ISensorGrain sensorGrain = clusterClient.GetGrain<ISensorGrain>(item.DeviceSensorId);
            await sensorGrain.RecordValue(item);

            SensorStateSummary state = await sensorGrain.GetDeviceState();
            await Clients.Group($"Sensor_{item.DeviceSensorId}").SendAsync("CurrentState", state);
        }
    }
}
