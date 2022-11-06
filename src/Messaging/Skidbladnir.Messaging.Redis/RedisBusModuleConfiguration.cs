namespace Skidbladnir.Messaging.Redis
{
    public class RedisBusModuleConfiguration
    {
        public string ConnectionString { get; set; }

        public string VirtualHost { get; set; }
    }
}