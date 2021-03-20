namespace Skidbladnir.Storage.Abstractions
{
    /// <summary>
    /// Storage information
    /// </summary>
    public interface IStorageInfo
    {
        /// <summary>
        /// Storage name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Storage type
        /// </summary>
        StorageType Type { get; }
    }
}