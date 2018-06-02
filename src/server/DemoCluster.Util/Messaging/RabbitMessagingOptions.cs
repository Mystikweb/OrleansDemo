
namespace DemoCluster.Util.Messaging
{
    public class RabbitMessagingOptions
    {
        public const string SECTION_NAME = "RabbitMessaging";

        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public string Namespace { get; set; }
        public string Exchange { get; set; }
    }
}