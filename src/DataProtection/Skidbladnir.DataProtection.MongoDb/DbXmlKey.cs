namespace Skidbladnir.DataProtection.MongoDb
{
    internal class DbXmlKey
    {
        public DbXmlKey(string id, string keyId, string key)
        {
            Id = id;
            KeyId = keyId;
            Key = key;
        }

        public string KeyId { get; internal set; }

        public string Key { get; internal set; }

        public string Id { get; internal set; }
    }
}