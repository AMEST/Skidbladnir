using Skidbladnir.Storage.Abstractions;

namespace Skidbladnir.Storage.S3
{
    public class S3StorageInfo : IStorageInfo
    {
        public S3StorageInfo(string serviceUrl,
                     string accessKey,
                     string secretKey,
                     string bucket)
        {
            Name = "S3 Storage";
            ServiceUrl = serviceUrl;
            AccessKey = accessKey;
            SecretKey = secretKey;
            Bucket = bucket;
        }

        public S3StorageInfo(string name,
                             string serviceUrl,
                             string accessKey,
                             string secretKey,
                             string bucket)
        {
            Name = name;
            ServiceUrl = serviceUrl;
            AccessKey = accessKey;
            SecretKey = secretKey;
            Bucket = bucket;
        }

        public string Name { get; }
        public string ServiceUrl { get; }
        public string AccessKey { get; }
        public string SecretKey { get; }
        public string Bucket { get; }
        public StorageType Type { get; } = StorageType.Remote;
    }
}