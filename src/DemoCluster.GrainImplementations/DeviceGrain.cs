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
        private bool IsRunning => State != null && State.CurrentState != null && State.CurrentState.Name == "RUNNING" ? true : false;
        private bool IsStopped => State != null && State.CurrentState != null && State.CurrentState.Name == "STOPPED" ? true : false;

        public DeviceGrain(ILogger<DeviceGrain> logger)
        {
            this.logger = logger;
        }

        public override async Task OnActivateAsync()
        {
            await RefreshNow();

            if (State != null && State.CurrentState != null && State.CurrentState.Name == "RUNNING")
            {
                await SetupReminder();
            }
        }

        public async Task ReceiveReminder(string reminderName, TickStatus status)
        {
            switch (reminderName)
            {
                case Constants.REMINDER_GET_SENSOR_UPDATES:
                    await UpdateSensorSummaries();
                    break;
            }
        }

        public async Task<DeviceSummaryViewModel> GetDeviceSummary()
        {
            DeviceSummaryViewModel result = new DeviceSummaryViewModel
            {
                DeviceId = deviceModel.DeviceId,
                Name = deviceModel.Name,
                IsRunning = IsRunning,
                Timestamp = State.Timestamp
            };

            foreach (DeviceSensorState deviceSensor in State.Sensors)
            {
                ISensorGrain sensor = GrainFactory.GetGrain<ISensorGrain>(deviceSensor.DeviceSensorId);
                SensorSummaryViewModel summary = await sensor.GetSummary();
                result.Sensors.Add(summary);
            }

            return result;
        }

        public Task<DeviceViewModel> GetDeviceModel()
        {
            return Task.FromResult(deviceModel);
        }

        public Task SetDeviceModel(DeviceViewModel model)
        {
            deviceModel = model;

            return Task.CompletedTask;
        }

        public Task<CurrentDeviceStateViewModel> GetCurrentStatus()
        {
            return Task.FromResult(State.CurrentState.ToViewModel(this.GetPrimaryKey()));
        }

        public async Task<bool> UpdateDevice(DeviceViewModel model, bool runDevice = true)
        {
            deviceModel = model;

            RaiseEvent(new UpdateDevice(this.GetPrimaryKey(), deviceModel.Name, deviceModel.IsEnabled));
            await ConfirmEvents();

            await ProcessConfigStatus(runDevice);
            await ProcessConfigSensors();

            return true;
        }

        public async Task<bool> UpdateDeviceState(DeviceStateViewModel state)
        {
            RaiseEvent(new UpdateDeviceState(state.DeviceStateId.Value, state.StateId, state.StateName));
            await ConfirmEvents();

            return true;
        }

        public async Task<bool> Start(DeviceViewModel model = null)
        {
            if (deviceModel == null && model != null)
            {
                await UpdateDevice(model);
            }
            else if (model == null && deviceModel != null)
            {
                await ProcessConfigStatus(true);
                await ProcessConfigSensors(true);
            }
            else
            {
                logger.LogError($"No configuration defined for device {this.GetPrimaryKeyString()}. Unable to start device.");
                return false;
            }

            return IsRunning;
        }

        public async Task<bool> Stop()
        {
            await ProcessConfigStatus(false);
            await ProcessConfigSensors(false);

            return IsStopped;
        }

        private async Task ProcessConfigStatus(bool shouldRun = true)
        {
            if (State.IsEnabled && shouldRun && !IsRunning)
            {
                DeviceStateViewModel runningState = deviceModel.States.FirstOrDefault(s => s.IsEnabled && s.StateName == "RUNNING");
                if (runningState != null)
                {
                    await UpdateDeviceState(runningState);
                }

                await SetupReminder();
            }

            if ((!State.IsEnabled || !shouldRun) && !IsStopped)
            {
                DeviceStateViewModel stoppedState = deviceModel.States.FirstOrDefault(s => s.IsEnabled && s.StateName == "STOPPED");
                if (stoppedState != null)
                {
                    await UpdateDeviceState(stoppedState);
                }

                await UnregisterReminder(reminder);
            }
        }

        private async Task ProcessConfigSensors(bool shouldRun = true)
        {
            foreach (DeviceSensorViewModel deviceSensorModel in deviceModel.Sensors)
            {
                ISensorGrain sensorGrain = GrainFactory.GetGrain<ISensorGrain>(deviceSensorModel.DeviceSensorId.Value);
                bool isRunning = await sensorGrain.UpdateModel(deviceSensorModel, shouldRun);

                if (!isRunning && shouldRun)
                {
                    logger.LogError($"Unable to start sensor {deviceSensorModel.SensorName} ({deviceSensorModel.DeviceSensorId}) with device {deviceModel.Name}");
                }
                else
                {
                    SensorSummaryViewModel sensorSummary = await sensorGrain.GetSummary();

                    RaiseEvent(new UpdateDeviceSensor(sensorSummary.DeviceSensorId, sensorSummary.SensorId, sensorSummary.SensorName,
                        sensorSummary.UOM, sensorSummary.IsEnabled, sensorSummary.IsRunning, sensorSummary.LastValue, sensorSummary.LastValueReceived,
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
                        sensorSummary.UOM, sensorSummary.IsEnabled, sensorSummary.IsRunning, sensorSummary.LastValue, sensorSummary.LastValueReceived,
                        sensorSummary.AverageValue, sensorSummary.TotalValue));

                    await ConfirmEvents();
                }
            }
        }

        private async Task SetupReminder()
        {
            reminder = await RegisterOrUpdateReminder(Constants.REMINDER_GET_SENSOR_UPDATES, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(5));
        }
    }
}