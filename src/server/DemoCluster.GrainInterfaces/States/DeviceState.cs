using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCluster.GrainInterfaces.States
{
    [Serializable]
    public class DeviceState
    {
        public Guid DeviceId { get; set; }
        public string Name { get; set; }
        public bool IsRunning { get; set; } = false;

        public HashSet<ISensorGrain> RegisteredSensors { get; set; } = new HashSet<ISensorGrain>();
        public SortedDictionary<DateTime, DeviceHistoryState> History { get; set; } = new SortedDictionary<DateTime, DeviceHistoryState>();

        public void Apply(DeviceHistoryState historyState)
        {
            if (historyState == null)
                throw new ArgumentNullException("deviceState");

            if (History.ContainsKey(historyState.Timestamp))
                return;

            History.Add(historyState.Timestamp, historyState);
        }
    }
}
