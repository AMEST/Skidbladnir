using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Skidbladnir.Storage.Abstractions;

namespace Skidbladnir.Storage.S3
{
    public class S3Storage<TStorageInfo> : IStorage<TStorageInfo> where TStorageInfo : S3StorageInfo
    {
        private readonly AmazonS3Client _client;
        private readonly TStorageInfo _storageInfo;

        public S3Storage(TStorageInfo storageInfo)
        {
            
            var config = new AmazonS3Config()
            {
                ServiceURL = storageInfo.ServiceUrl
            };
            _client = new AmazonS3Client(
                storageInfo.AccessKey, storageInfo.SecretKey,
                config
            );
            _storageInfo = storageInfo;
        }

        public async Task CopyAsync(string srcPath, string destPath)
        {
            await EnsureBucketCreated();
            var request = new CopyObjectRequest()
            {
                SourceBucket = _storageInfo.Bucket,
                DestinationBucket = _storageInfo.Bucket,
                SourceKey = srcPath.NormalizePath(),
                DestinationKey = destPath.NormalizePath()
            };
            await _client.CopyObjectAsync(request);
        }

        public async Task DeleteAsync(string pathToFile)
        {
            await EnsureBucketCreated();
            var request = new DeleteObjectRequest()
            {
                BucketName = _storageInfo.Bucket,
                Key = pathToFile.NormalizePath()
            };
            await _client.DeleteObjectAsync(request);
        }

        public async Task<DownloadResult> DownloadFileAsync(string pathToFile)
        {
            await EnsureBucketCreated();
            var request = new GetObjectRequest()
            {
                BucketName = _storageInfo.Bucket,
                Key = pathToFile.NormalizePath()
            };
            var result = await _client.GetObjectAsync(request);
            var fileInfo = new Abstractions.FileInfo(pathToFile.NormalizePath(), result.ContentLength, result.LastModified);
            return new DownloadResult(fileInfo, result.ResponseStream);
        }

        public async Task<bool> Exist(string path)
        {
            await EnsureBucketCreated();
            var request = new GetObjectMetadataRequest()
            {
                BucketName = _storageInfo.Bucket,
                Key = path.NormalizePath()
            };
            try
            {
                var result = await _client.GetObjectMetadataAsync(request);
                return true;
            }
            catch (AmazonS3Exception)
            {
                return false;
            }
        }

        public async Task<Abstractions.FileInfo> GetFileAsync(string pathToFile)
        {
            await EnsureBucketCreated();
            var request = new GetObjectMetadataRequest()
            {
                BucketName = _storageInfo.Bucket,
                Key = pathToFile.NormalizePath()
            };
            var result = await _client.GetObjectMetadataAsync(request);
            return new Abstractions.FileInfo(pathToFile.NormalizePath(), result.ContentLength, result.LastModified);
        }

        public async Task<Abstractions.FileInfo[]> GetFilesAsync(string path)
        {
            await EnsureBucketCreated();
            var request = new ListObjectsRequest()
            {
                BucketName = _storageInfo.Bucket,
                Prefix = path.NormalizePath()
            };
            var result = await _client.ListObjectsAsync(request);
            return result.S3Objects.Select(o =>
                    new Abstractions.FileInfo(o.Key, o.Size, o.LastModified)
                ).ToArray();
        }

        public async Task MoveAsync(string srcPath, string destPath)
        {
            await CopyAsync(srcPath, destPath);
            await DeleteAsync(srcPath);
        }

        public async Task<Abstractions.FileInfo> UploadFileAsync(Stream stream, string pathToFile)
        {
            await EnsureBucketCreated();
            var request = new PutObjectRequest()
            {
                BucketName = _storageInfo.Bucket,
                Key = pathToFile.NormalizePath(),
                InputStream = stream
            };
            var result = await _client.PutObjectAsync(request);
            return new Abstractions.FileInfo(pathToFile.NormalizePath(), result.ContentLength, DateTime.UtcNow);
        }

        private async Task EnsureBucketCreated()
        {
            var bucketSearchRequest = new ListBucketsRequest();
            var buckets = await _client.ListBucketsAsync(bucketSearchRequest);
            if(buckets.Buckets.Any(x => x.BucketName == _storageInfo.Bucket))
                return;
            
            var bucketCreateRequest = new PutBucketRequest(){
                BucketName = _storageInfo.Bucket
            };
            await _client.PutBucketAsync(bucketCreateRequest);
        }
    }
}