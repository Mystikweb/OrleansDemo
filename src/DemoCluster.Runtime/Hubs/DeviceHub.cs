using DemoCluster.GrainInterfaces;
using DemoCluster.Models;
using Microsoft.AspNetCore.SignalR;
using Orleans;
using System;
using System.Threading.Tasks;

namespace DemoCluster.Runtime.Hubs
{
    public class DeviceHub : Hub
    {
        private readonly IGrainFactory grainFactory;

        public DeviceHub(IGrainFactory grainFactory)
        {
            this.grainFactory = grainFactory;
        }

        public async Task RegisterDeviceUpdates(string deviceId)
        {
            IDeviceGrain deviceGrain = grainFactory.GetGrain<IDeviceGrain>(Guid.Parse(deviceId));
            DeviceSummaryViewModel summary = await deviceGrain.GetDeviceSummary();

            await Groups.AddToGroupAsync(Context.ConnectionId, deviceId);
            await Clients.Group(deviceId).SendAsync(Constants.MESSAGING_DEVICE_SUMMARY, summary);
        }

        public async Task UnregisterDeviceUpdates(string deviceId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, deviceId);
        }

        public async Task StartDevice(string deviceId)
        {
            IDeviceGrain deviceGrain = grainFactory.GetGrain<IDeviceGrain>(Guid.Parse(deviceId));

            bool didStart = await deviceGrain.Start();

            if (didStart)
            {
                DeviceSummaryViewModel summary = await deviceGrain.GetDeviceSummary();
                await Clients.Group(deviceId).SendAsync(Constants.MESSAGING_DEVICE_SUMMARY, summary);
            }
            else
            {
                await Clients.Group(deviceId).SendAsync(Constants.MESSAGING_DEVICE_ERROR, $"Error starting device.");
            }
        }

        public async Task StopDevice(string deviceId)
        {
            IDeviceGrain deviceGrain = grainFactory.GetGrain<IDeviceGrain>(Guid.Parse(deviceId));

            bool didStop = await deviceGrain.Stop();

            if (didStop)
            {
                DeviceSummaryViewModel summary = await deviceGrain.GetDeviceSummary();
                await Clients.Group(deviceId).SendAsync(Constants.MESSAGING_DEVICE_SUMMARY, summary);
            }
            else
            {
                await Clients.Group(deviceId).SendAsync(Constants.MESSAGING_DEVICE_ERROR, $"Error starting device.");
            }
        }
    }
}
