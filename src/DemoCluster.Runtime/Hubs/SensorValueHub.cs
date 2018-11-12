using DemoCluster.GrainInterfaces;
using DemoCluster.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Orleans;
using System.Threading.Tasks;

namespace DemoCluster.Runtime.Hubs
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
            SensorSummaryViewModel summary = await sensorGrain.GetSummary();
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Sensor_{deviceSensorId}");

            await Clients.Group($"Sensor_{deviceSensorId}").SendAsync(Constants.MESSAGING_SENSOR_VALUE_CURRENT_STATE, summary);
        }

        public async Task RecordValue(SensorValueViewModel value)
        {
            ISensorGrain sensorGrain = clusterClient.GetGrain<ISensorGrain>(value.DeviceSensorId);
            await sensorGrain.RecordValue(value);

            SensorSummaryViewModel summary = await sensorGrain.GetSummary();
            await Clients.Group($"Sensor_{value.DeviceSensorId}").SendAsync(Constants.MESSAGING_SENSOR_VALUE_CURRENT_STATE, summary);
        }
    }
}
