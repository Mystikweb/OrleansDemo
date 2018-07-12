using System;
using System.Collections.Generic;
using DemoCluster.GrainInterfaces.Commands;

namespace DemoCluster.GrainInterfaces.States
{
    [Serializable]
    public class MessageMonitorState
    {
        public string ExchangeName { get; set; }
        public string QueueName { get; set; }
        public DateTime Timestamp { get; set; }
        public List<MessageValue> History { get; set; } = new List<MessageValue>();

        public void Apply(MessageMonitorSetupCommand @event)
        {
            ExchangeName = @event.Exchange;
            QueueName = @event.Queue;
            Timestamp = @event.Timestamp;
        }

        public void Apply(MessageValueReceivedCommand @event)
        {
            History.Add(new MessageValue { Message = @event.Message, Timestamp = @event.Timestamp });
        }
    }

    [Serializable]
    public class MessageValue
    {
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}