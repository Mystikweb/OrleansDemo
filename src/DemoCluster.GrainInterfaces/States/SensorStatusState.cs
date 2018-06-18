using System;
using System.Collections.Generic;
using DemoCluster.GrainInterfaces.Commands;

namespace DemoCluster.GrainInterfaces.States
{
    [Serializable]
    public class SensorStatusState
    {
        public int DeviceSensorId { get; set; }
        public string DeviceName { get; set; }
        public string SensorName { get; set; }
        public string Uom { get; set; }
        public bool IsReceiving { get; set; }
        public DateTime Timestamp { get; set; }
        public int Version { get; set; } = 0;
        public SortedDictionary<int, SensorStatusHistory> History { get; set; } = new SortedDictionary<int, SensorStatusHistory>();

        public void Apply(SensorStatusCommand update)
        {
            this.IsReceiving = update.IsReceiving;
            this.Timestamp = update.Timestamp;
            this.Version = update.Version.HasValue ? update.Version.Value : this.Version++;

            if (this.History.ContainsKey(this.Version))
            {
                return;
            }

            this.History.Add(this.Version, new SensorStatusHistory
            {
                IsReceiving = this.IsReceiving,
                Timestamp = this.Timestamp,
                Version = this.Version
            });
        }
    }

    [Serializable]
    public class SensorStatusHistory
    {
        public bool IsReceiving { get; set; }
        public DateTime Timestamp { get; set; }
        public int Version { get; set; }
    }
}