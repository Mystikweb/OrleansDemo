using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCluster.GrainInterfaces.States
{
    [Serializable]
    public class SensorState
    {
        public int DeviceSensorId { get; set; }
        public string Name { get; set; }
        public string UOM { get; set; }
        public bool IsReceiving { get; set; } = false;

        public SortedDictionary<DateTime, SensorHistoryState> History { get; set; } = new SortedDictionary<DateTime, SensorHistoryState>();

        public void Apply(SensorHistoryState historyState)
        {
            if (historyState == null)
                throw new ArgumentNullException("historyState");

            if (History.ContainsKey(historyState.Timestamp))
                return;

            History.Add(historyState.Timestamp, historyState);
        }
    }
}
