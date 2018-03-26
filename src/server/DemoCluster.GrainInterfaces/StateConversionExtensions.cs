using DemoCluster.DAL.Models;
using DemoCluster.GrainInterfaces.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCluster.GrainInterfaces
{
    public static class StateConversionExtensions
    {
        public static DeviceState ToState(this DeviceStateItem item)
        {
            var result = new DeviceState
            {
                DeviceId = Guid.Parse(item.DeviceId),
                Name = item.Name
            };

            foreach (var sensor in item.Sensors)
            {
                result.Sensors.Add(sensor.ToState());
            }

            return result;
        }

        public static SensorState ToState(this SensorStateItem item)
        {
            return new SensorState
            {
                DeviceSensorId = item.DeviceSensorId,
                Name = item.Name,
                UOM = item.UOM
            };
        }

        public static DeviceHistoryState ToState(this DeviceHistoryItem item)
        {
            return new DeviceHistoryState
            {
                DeviceId = item.DeviceId,
                Timestamp = item.TimeStamp,
                IsRunning = item.IsRunning,
                SensorCount = item.SensorCount,
                EventTypeCount = item.EventTypeCount
            };
        }

        public static DeviceHistoryItem ToItem(this DeviceHistoryState state, string name)
        {
            return new DeviceHistoryItem
            {
                DeviceId = state.DeviceId,
                Name = name,
                IsRunning = state.IsRunning,
                TimeStamp = state.Timestamp,
                SensorCount = state.SensorCount,
                EventTypeCount = state.EventTypeCount
            };
        }
    }
}
