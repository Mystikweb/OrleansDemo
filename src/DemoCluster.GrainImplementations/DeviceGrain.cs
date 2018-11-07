using DemoCluster.Commands;
using DemoCluster.GrainInterfaces;
using DemoCluster.Models;
using DemoCluster.States;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.EventSourcing;
using Orleans.Providers;
using Orleans.Runtime;
using System;
using System.Linq;
using System.Threading.Tasks;

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
        private DeviceViewModel deviceModel;

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

        public Task<DeviceViewModel> GetDeviceModel()
        {
            return Task.FromResult(deviceModel);
        }

        public Task<CurrentDeviceStateViewModel> GetCurrentStatus()
        {
            return Task.FromResult(State.CurrentState.ToViewModel(this.GetPrimaryKey()));
        }

        public async Task<bool> UpdateDevice(DeviceViewModel config)
        {
            deviceModel = config;

            RaiseEvent(new UpdateDevice(this.GetPrimaryKey(), deviceModel.Name));
            await ConfirmEvents();
            await ProcessConfigStatus();
            await ProcessConfigSensors();

            return true;
        }

        public async Task<bool> UpdateDeviceState(DeviceStateViewModel state)
        {
            RaiseEvent(new UpdateDeviceState(state.DeviceStateId.Value, state.StateId, state.StateName));
            await ConfirmEvents();

            return true;
        }

        private async Task ProcessConfigStatus()
        {
            if (deviceModel.IsEnabled && State.CurrentState.Name != "RUNNING")
            {
                DeviceStateViewModel runningState = deviceModel.States.FirstOrDefault(s => s.IsEnabled && s.StateName == "RUNNING");
                if (runningState != null)
                {
                    await UpdateDeviceState(runningState);
                }

                reminder = await RegisterOrUpdateReminder("GetSensorUpdates", TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(5));
            }

            if (!deviceModel.IsEnabled && State.CurrentState.Name != "STOPPED")
            {
                DeviceStateViewModel stoppedState = deviceModel.States.FirstOrDefault(s => s.IsEnabled && s.StateName == "STOPPED");
                if (stoppedState != null)
                {
                    await UpdateDeviceState(stoppedState);
                }

                await UnregisterReminder(reminder);
            }
        }

        private async Task ProcessConfigSensors()
        {
            foreach (DeviceSensorViewModel deviceSensorModel in deviceModel.Sensors)
            {
                ISensorGrain sensorGrain = GrainFactory.GetGrain<ISensorGrain>((long)deviceSensorModel.DeviceSensorId.Value);
                bool isSetup = await sensorGrain.UpdateModel(deviceSensorModel);

                if (!isSetup)
                {
                    logger.LogError($"Unable to register sensor {deviceSensorModel.SensorName} ({deviceSensorModel.DeviceSensorId}) with device {deviceModel.Name}");
                }
                else
                {
                    SensorSummaryViewModel sensorSummary = await sensorGrain.GetSummary();

                    RaiseEvent(new UpdateDeviceSensor(sensorSummary.DeviceSensorId, sensorSummary.SensorId, sensorSummary.SensorName,
                        sensorSummary.UOM, sensorSummary.IsEnabled, sensorSummary.LastValue, sensorSummary.LastValueReceived,
                        sensorSummary.AverageValue, sensorSummary.TotalValue));

                    await ConfirmEvents();
                }
            }
        }

        private async Task UpdateSensorSummaries()
        {
            if (deviceModel != null)
            {
                foreach (DeviceSensorViewModel sensor in deviceModel.Sensors)
                {
                    ISensorGrain sensorGrain = GrainFactory.GetGrain<ISensorGrain>((long)sensor.DeviceSensorId.Value);
                    SensorSummaryViewModel sensorSummary = await sensorGrain.GetSummary();

                    RaiseEvent(new UpdateDeviceSensor(sensorSummary.DeviceSensorId, sensorSummary.SensorId, sensorSummary.SensorName,
                        sensorSummary.UOM, sensorSummary.IsEnabled, sensorSummary.LastValue, sensorSummary.LastValueReceived,
                        sensorSummary.AverageValue, sensorSummary.TotalValue));

                    await ConfirmEvents();
                }
            }
        }
    }
}