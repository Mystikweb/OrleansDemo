using DemoCluster.Commands;
using DemoCluster.GrainInterfaces;
using DemoCluster.Models;
using DemoCluster.States;
using Microsoft.Extensions.Logging;
using Orleans.EventSourcing;
using Orleans.Providers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DemoCluster.GrainImplementations
{
    [LogConsistencyProvider(ProviderName="LogStorage")]
    [StorageProvider(ProviderName = "SqlBase")]
    public class SensorGrain :
        JournaledGrain<SensorState>,
        ISensorGrain
    {
        private readonly ILogger logger;

        public SensorGrain(ILogger<SensorGrain> logger)
        {
            this.logger = logger;
        }

        public override async Task OnActivateAsync()
        {
            await RefreshNow();
        }

        public async Task<bool> UpdateModel(DeviceSensorViewModel model)
        {
            RaiseEvent(new UpdateSensor(model.DeviceSensorId.Value, model.SensorId, model.SensorName, model.UOM, model.IsEnabled));
            await ConfirmEvents();

            return true;
        }

        public Task<SensorSummaryViewModel> GetSummary()
        {
            SensorSummaryViewModel result = State.ToViewModel();
            SensorValueState lastValue = State.Values.OrderByDescending(v => v.Timestamp).FirstOrDefault();

            result.LastValue = lastValue == null ? null : (double?)lastValue.Value;
            result.LastValueReceived = lastValue == null ? null : (DateTime?)lastValue.Timestamp;

            result.AverageValue = State.Values.Count > 0 ? (double?)State.Values.Average(v => v.Value) : null;
            result.TotalValue = State.Values.Count > 0 ? (double?)State.Values.Sum(v => v.Value) : null;

            return Task.FromResult(result);
        }

        public async Task RecordValue(SensorValueViewModel value)
        {
            RaiseEvent(new AddSensorValue(value.Value, value.TimeStamp));
            await ConfirmEvents();

            logger.LogInformation($"Sensor {State.Name} ({State.DeviceSensorId}) received new value of {value.Value} at {value.TimeStamp.ToLocalTime().ToLongTimeString()}");
        }
    }
}