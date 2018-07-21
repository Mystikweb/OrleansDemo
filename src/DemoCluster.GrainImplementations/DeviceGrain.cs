using System;
using System.Linq;
using System.Threading.Tasks;
using DemoCluster.DAL;
using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces;
using DemoCluster.GrainInterfaces.Commands;
using DemoCluster.GrainInterfaces.States;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.EventSourcing;
using Orleans.Providers;
using Orleans.Runtime;

namespace DemoCluster.GrainImplementations
{
    [LogConsistencyProvider(ProviderName="LogStorage")]
    [StorageProvider(ProviderName = "SqlBase")]
    public class DeviceGrain : 
        JournaledGrain<DeviceState>, IRemindable,
        IDeviceGrain
    {
        private readonly ILogger logger;
        private IGrainReminder reminder;
        private DeviceConfig deviceConfig;

        public DeviceGrain(ILogger<DeviceGrain> logger)
        {
            this.logger = logger;
        }

        public override async Task OnActivateAsync()
        {
            await RefreshNow();

            if (State.CurrentState != null && State.CurrentState.Name == "RUNNING")
            {
                reminder = await RegisterOrUpdateReminder("GetSensorUpdates", TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(5));
            }
        }

        public async Task ReceiveReminder(string reminderName, TickStatus status)
        {
            switch (reminderName)
            {
                case "GetSensorUpdates":
                    await UpdateSensorSummaries();
                    break;
            }
        }

        public Task<Guid> GetKey()
        {
            return Task.FromResult(this.GetPrimaryKey());
        }

        public Task<DeviceState> GetState()
        {
            return Task.FromResult(State);
        }

        public Task<DeviceConfig> GetCurrentConfig()
        {
            return Task.FromResult(deviceConfig);
        }

        public Task<DeviceStateItem> GetCurrentStatus()
        {
            return Task.FromResult(State.CurrentState.ToStateItem(this.GetPrimaryKey()));
        }

        public async Task<bool> UpdateConfig(DeviceConfig config)
        {
            deviceConfig = config;

            RaiseEvent(new DeviceConfigCommand(this.GetPrimaryKey(), deviceConfig.Name));
            await ConfirmEvents();
            await ProcessConfigStatus();
            await ProcessConfigSensors();

            return true;
        }

        public async Task<bool> UpdateCurrentStatus(DeviceStateItem state)
        {
            RaiseEvent(new DeviceStatusCommand(state.DeviceStatusId, state.StatusId, state.Name));
            await ConfirmEvents();

            return true;
        }

        private async Task ProcessConfigStatus()
        {
            if (deviceConfig.IsEnabled && State.CurrentState.Name != "RUNNING")
            {
                var runningState = deviceConfig.States.FirstOrDefault(s => s.IsEnabled && s.Name == "RUNNING");
                if (runningState != null)
                {
                    await UpdateCurrentStatus(runningState.ConfigToStateItem(this.GetPrimaryKey()));
                }

                reminder = await RegisterOrUpdateReminder("GetSensorUpdates", TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(5));
            }

            if (!deviceConfig.IsEnabled && State.CurrentState.Name != "STOPPED")
            {
                var stoppedState = deviceConfig.States.FirstOrDefault(s => s.IsEnabled && s.Name == "STOPPED");
                if (stoppedState != null)
                {
                    await UpdateCurrentStatus(stoppedState.ConfigToStateItem(this.GetPrimaryKey()));
                }

                await UnregisterReminder(reminder);
            }
        }

        private async Task ProcessConfigSensors()
        {
            foreach (var sensor in deviceConfig.Sensors)
            {
                var sensorGrain = GrainFactory.GetGrain<ISensorGrain>((long)sensor.DeviceSensorId);
                var isSetup = await sensorGrain.UpdateConfig(sensor);

                if (!isSetup)
                {
                    logger.LogError($"Unable to register sensor {sensor.Name} ({sensor.DeviceSensorId}) with device {deviceConfig.Name}");
                }
                else
                {
                    var sensorSummary = await sensorGrain.GetDeviceState();

                    RaiseEvent(new DeviceSensorSummaryUpdatedCommand(sensorSummary.DeviceSensorId, sensorSummary.SensorId, sensorSummary.Name,
                        sensorSummary.UOM, sensorSummary.IsEnabled, sensorSummary.LastValue, sensorSummary.LastValueReceived,
                        sensorSummary.AverageValue, sensorSummary.TotalValue));

                    await ConfirmEvents();
                }
            }
        }

        private async Task UpdateSensorSummaries()
        {
            if (deviceConfig != null)
            {
                foreach (var sensor in deviceConfig.Sensors)
                {
                    var sensorGrain = GrainFactory.GetGrain<ISensorGrain>((long)sensor.DeviceSensorId);
                    var sensorSummary = await sensorGrain.GetDeviceState();

                    RaiseEvent(new DeviceSensorSummaryUpdatedCommand(sensorSummary.DeviceSensorId, sensorSummary.SensorId, sensorSummary.Name,
                        sensorSummary.UOM, sensorSummary.IsEnabled, sensorSummary.LastValue, sensorSummary.LastValueReceived,
                        sensorSummary.AverageValue, sensorSummary.TotalValue));

                    await ConfirmEvents();
                }
            }
        }
    }
}