using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Skidbladnir.Storage.Abstractions;
using FileInfo = Skidbladnir.Storage.Abstractions.FileInfo;

namespace Skidbladnir.Storage.GridFS
{
    internal class GridFsStorage<TStorageInfo> : IStorage<TStorageInfo> where TStorageInfo : GridFsStorageInfo
    {
        private readonly Lazy<IGridFSBucket> _gridFsBucket;

        public GridFsStorage(BucketFactory<TStorageInfo> bucketFactory)
        {
            _gridFsBucket = new Lazy<IGridFSBucket>(bucketFactory.Create);
        }

        public async Task<FileInfo[]> GetFilesAsync(string path)
        {
            var filter =
                Builders<GridFSFileInfo>.Filter.Regex(f => f.Filename,
                    new BsonRegularExpression($"^{path.NormilizePath()}.*"));
            var results = await (await FindAsync(filter).ConfigureAwait(false)).ToListAsync().ConfigureAwait(false);

            return results.Select(Extensions.ToFileInfo).ToArray();
        }

        public async Task<FileInfo> GetFileAsync(string pathToFile)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq(f => f.Filename, pathToFile.NormilizePath());
            var results = await FindAsync(filter).ConfigureAwait(false);
            var info = await results.FirstOrDefaultAsync();
            return info?.ToFileInfo();
        }

        public async Task CopyAsync(string srcPath, string destPath)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq(f => f.Filename, srcPath.NormilizePath());
            var results = await (await FindAsync(filter).ConfigureAwait(false)).ToListAsync().ConfigureAwait(false);

            if (!results.Any())
                throw new FileNotFoundException("File not found", srcPath);

            if (await Exist(destPath.NormilizePath()).ConfigureAwait(false))
                await DeleteAsync(destPath).ConfigureAwait(false);

            using (var fileStream = _gridFsBucket.Value.OpenDownloadStream(results.First().Id))
            {
                await _gridFsBucket.Value.UploadFromStreamAsync(destPath.NormilizePath(), fileStream);
            }
        }

        public async Task MoveAsync(string srcPath, string destPath)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq(f => f.Filename, srcPath.NormilizePath());
            var results = await (await FindAsync(filter).ConfigureAwait(false)).ToListAsync().ConfigureAwait(false);

            if (!results.Any())
                return;

            if (await Exist(destPath.NormilizePath()).ConfigureAwait(false))
                await DeleteAsync(destPath).ConfigureAwait(false);

            await _gridFsBucket.Value.RenameAsync(results.First().Id, destPath.NormilizePath());
        }

        public async Task DeleteAsync(string pathToFile)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq(f => f.Filename, pathToFile.NormilizePath());
            var results = await (await FindAsync(filter).ConfigureAwait(false)).ToListAsync().ConfigureAwait(false);

            if (!results.Any())
                return;

            await _gridFsBucket.Value.DeleteAsync(results.First().Id).ConfigureAwait(false);
        }

        public async Task<bool> Exist(string path)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, path.NormilizePath());
            var results = await FindAsync(filter)
                .ConfigureAwait(false);
            return await results.AnyAsync().ConfigureAwait(false);
        }

        public async Task<FileInfo> UploadFileAsync(Stream stream, string pathToFile)
        {
            if (await Exist(pathToFile.NormilizePath()).ConfigureAwait(false))
                await DeleteAsync(pathToFile).ConfigureAwait(false);

            await _gridFsBucket.Value.UploadFromStreamAsync(pathToFile.NormilizePath(), stream).ConfigureAwait(false);

            return await GetFileAsync(pathToFile).ConfigureAwait(false);
        }

        public async Task<DownloadResult> DownloadFileAsync(string pathToFile)
        {
            var info = await GetFileAsync(pathToFile).ConfigureAwait(false);
            if (info == null)
                throw new FileNotFoundException("File not found", pathToFile.NormilizePath());

            var downloadStream = new MemoryStream();
            await _gridFsBucket.Value.DownloadToStreamByNameAsync(pathToFile.NormilizePath(), downloadStream).ConfigureAwait(false);

            downloadStream.Position = 0;
            return new DownloadResult(info, downloadStream);
        }

        private Task<IAsyncCursor<GridFSFileInfo>> FindAsync(FilterDefinition<GridFSFileInfo> filter)
        {
            return _gridFsBucket.Value.FindAsync(filter);
        }
    }
}