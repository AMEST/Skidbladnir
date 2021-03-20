using Skidbladnir.Storage.Abstractions;

namespace Skidbladnir.Storage.LocalFileStorage
{
    /// <summary>
    /// Local storage (local fs) info
    /// </summary>
    public class LocalStorageInfo : IStorageInfo
    {
        public LocalStorageInfo(string storagePath)
        
        {
            Name = "Local storage";
            StoragePath = storagePath;
            Type = StorageType.Local;
        }

        public LocalStorageInfo(string name, string storagePath)
        {
            Name = name;
            StoragePath = storagePath;
            Type = StorageType.Local;
        }

        public string Name { get; }
        public StorageType Type { get; }
        public string StoragePath { get; }
    }
}