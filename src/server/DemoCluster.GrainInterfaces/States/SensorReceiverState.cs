using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCluster.GrainInterfaces.States
{
    [Serializable]
    public class SensorReceiverState
    {
        public int DeviceSensorId { get; set; }

        public SortedDictionary<DateTime, SensorValueState> Values { get; set; } = new SortedDictionary<DateTime, SensorValueState>();

        public void Apply(SensorValueState valueState)
        {
            if (valueState == null)
                throw new ArgumentNullException("historyState");

            if (Values.ContainsKey(valueState.Timestamp))
                return;

            Values.Add(valueState.Timestamp, valueState);
        }
    }
}
