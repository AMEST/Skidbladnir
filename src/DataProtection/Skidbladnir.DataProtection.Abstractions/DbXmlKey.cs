using Skidbladnir.Repository.Abstractions;

namespace Skidbladnir.DataProtection.Abstractions
{
    public class DbXmlKey : IHasId
    {
        public DbXmlKey(string id, string keyId, string key)
        {
            Id = id;
            KeyId = keyId;
            Key = key;
        }

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        public string KeyId { get; private set; }

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        public string Key { get; private set; }

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        public string Id { get; set; }

    }
}