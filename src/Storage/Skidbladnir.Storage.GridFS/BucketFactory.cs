using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Skidbladnir.Storage.GridFS
{
    internal class BucketFactory<TStorageInfo> where TStorageInfo : GridFsStorageInfo
    {
        private readonly MongoUrl _mongoUrl;

        public BucketFactory(TStorageInfo storageInfo)
        {
            _mongoUrl = new MongoUrl(storageInfo.ConnectionString);
        }

        public IGridFSBucket Create()
        {
            var client = new MongoClient(_mongoUrl);
            var database = client.GetDatabase(_mongoUrl.DatabaseName);
            return new GridFSBucket(database);
        }
    }
}