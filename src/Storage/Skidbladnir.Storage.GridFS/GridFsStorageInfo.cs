using Skidbladnir.Storage.Abstractions;

namespace Skidbladnir.Storage.GridFS
{
    public class GridFsStorageInfo : IStorageInfo
    {
        public GridFsStorageInfo(string connectionString)

        {
            Name = "GridFS storage";
            ConnectionString = connectionString;
            Type = StorageType.Local;
        }

        public GridFsStorageInfo(string name, string connectionString)
        {
            Name = name;
            ConnectionString = connectionString;
            Type = StorageType.Local;
        }

        public string Name { get; }
        public StorageType Type { get; }
        public string ConnectionString { get; }
    }
}