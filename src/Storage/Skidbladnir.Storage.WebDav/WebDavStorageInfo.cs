using Skidbladnir.Storage.Abstractions;

namespace Skidbladnir.Storage.WebDav
{
    public class WebDavStorageInfo : IStorageInfo
    {
        public WebDavStorageInfo(WebDavStorageConfiguration configuration)
        {
            Configuration = configuration;
            Name = "WebDav";
            Type = StorageType.Remote;
        }

        public WebDavStorageInfo(string name, WebDavStorageConfiguration configuration)
        {
            Configuration = configuration;
            Name = name;
            Type = StorageType.Remote;
        }

        public WebDavStorageConfiguration Configuration { get; }
        
        public string Name { get; }

        public StorageType Type { get; }
    }
}