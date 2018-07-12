using System;

namespace DemoCluster.GrainInterfaces.Commands
{
    public interface IMessageMonitorCommand
    {
        DateTime Timestamp { get; }
    }

    public class MessageMonitorSetupCommand : IMessageMonitorCommand
    {
        public MessageMonitorSetupCommand(string name, string hostName, string userName, string password, string exchangeName, string queueName, DateTime? timeStamp = null)
        {
            Name = name;
            HostName = hostName;
            UserName = userName;
            Password = password;
            Exchange = exchangeName;
            Queue = queueName;
            Timestamp = timeStamp.HasValue ? timeStamp.Value : DateTime.UtcNow;        
        }

        public string Name { get; private set; }
        public string HostName { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string Exchange { get; private set; }
        public string Queue { get; private set; }
        public DateTime Timestamp { get; private set;}
    }

    public class MessageValueReceivedCommand : IMessageMonitorCommand
    {
        public MessageValueReceivedCommand(string message, DateTime? timeStamp = null)
        {
            Message = message;
            Timestamp = timeStamp.HasValue ? timeStamp.Value : DateTime.UtcNow;
        }
        
        public string Message { get; private set; }
        public DateTime Timestamp { get; private set;}
    }
}