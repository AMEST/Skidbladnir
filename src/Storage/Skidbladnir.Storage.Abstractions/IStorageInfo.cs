namespace Skidbladnir.Storage.Abstractions
{
    public interface IStorageInfo
    {
        string Name { get; }

        StorageType Type { get; }
    }
}