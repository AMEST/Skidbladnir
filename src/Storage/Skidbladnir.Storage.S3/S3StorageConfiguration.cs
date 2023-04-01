namespace Skidbladnir.Storage.S3
{
    public class S3StorageConfiguration
    {
        public string ServiceUrl { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string Bucket { get; set; }

    }
}