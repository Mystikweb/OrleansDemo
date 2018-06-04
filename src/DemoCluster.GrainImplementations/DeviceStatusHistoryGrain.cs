using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoCluster.DAL;
using DemoCluster.GrainInterfaces;
using DemoCluster.GrainInterfaces.Commands;
using DemoCluster.GrainInterfaces.States;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.EventSourcing;
using Orleans.EventSourcing.CustomStorage;
using Orleans.Providers;

namespace DemoCluster.GrainImplementations
{
    [LogConsistencyProvider(ProviderName="LogStorage")]
    public class DeviceStatusHistoryGrain :
        JournaledGrain<DeviceStatusState, DeviceStatusCommand>,
        ICustomStorageInterface<DeviceStatusState, DeviceStatusCommand>,
        IDeviceStatusHistoryGrain
    {
        private readonly IConfigurationStorage configStorage;
        private readonly IRuntimeStorage runtimeStorage;
        private readonly ILogger logger;

        public DeviceStatusHistoryGrain(IConfigurationStorage configStorage, 
            IRuntimeStorage runtimeStorage, ILogger<DeviceStatusHistoryGrain> logger)
        {
            this.configStorage = configStorage;
            this.runtimeStorage = runtimeStorage;
            this.logger = logger;
        }

        public override async Task OnActivateAsync()
        {
            var config = await configStorage.GetDeviceByIdAsync(this.GetPrimaryKey().ToString());
            State.DeviceId = Guid.Parse(config.DeviceId);
            State.DeviceName = config.Name;

            await RefreshNow();
        }

        public Task<CurrentStatusState> GetCurrentStatus()
        {
            return Task.FromResult(new CurrentStatusState
            {
                DeviceStateId = State.DeviceStateId,
                Name = State.Name
            });
        }

        public Task<List<DeviceStatusHistory>> GetStatusHistory()
        {
            var result = State.History.Select(h => h.Value).ToList();

            return Task.FromResult(result);
        }

        public async Task<bool> UpdateStatus(DeviceStatusCommand update)
        {
            RaiseEvent(update);
            await ConfirmEvents();

            return true;
        }

        public async Task<KeyValuePair<int, DeviceStatusState>> ReadStateFromStorage()
        {
            var historyItems = await runtimeStorage.GetDeviceStateHistory(this.GetPrimaryKey());

            int version = 0;
            foreach (var item in historyItems)
            {
                State.DeviceId = item.DeviceId;
                State.DeviceName = item.Name;
                State.Apply(item.CreateDeviceStatusCommand());
                version = item.Version;
            }

            return new KeyValuePair<int, DeviceStatusState>(version, State);
        }

        public async Task<bool> ApplyUpdatesToStorage(IReadOnlyList<DeviceStatusCommand> updates, int expectedversion)
        {
            int version = 0;
            try
            {
                foreach (var item in updates)
                {
                    await runtimeStorage.SaveDeviceState(item.CreateDeviceStateHistory(State.DeviceId, State.DeviceName));
                    version = item.Version.Value;
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error applying updates to storage");
                return false;
            }

            return version == expectedversion;
        }
    }
}