namespace Skidbladnir.DataProtection.MongoDb
{
    public class DataProtectionMongoModuleConfiguration
    {
        /// <summary>
        ///     MongoDB ConnectionString
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        ///     DataProtection Collection Name
        /// </summary>
        public string CollectionName { get; set; } = "dataProtection";
    }
}