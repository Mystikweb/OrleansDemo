using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCluster.DAL.States
{
    [Serializable]
    public class DeviceHistory
    {
        public SortedDictionary<DateTime, DeviceState> StateHistory { get; set; } = new SortedDictionary<DateTime, DeviceState>();

        public void Apply(DeviceState deviceState)
        {
            if (deviceState == null)
                throw new ArgumentNullException("deviceState");

            if (StateHistory.ContainsKey(deviceState.Timestamp))
                return;

            StateHistory.Add(deviceState.Timestamp, deviceState);
        }
    }
}
