# [Skidbladnir Home](../../README.md)

## Storage

### Description

This direction of the Skidbladnir library set will contain implementations of the File Storage Subsystem (`IStorage`) for data access.

The file storage subsystem is necessary to abstract from the implementation of access and storage of files, which allows you to change the storage, use different storages for different types of files with minimal changes to the source code and the ability to simultaneously use several storages (for example, local for the cache, GridFs for images, and webdav for archives that are rarely requested).

### IStorage abstraction methods

1. `Task<FileInfo[]> GetFilesAsync(string path);` - get all file from path in storage
1. `Task<FileInfo> GetFileAsync(string pathToFile);` - get info about file in storage
1. `Task CopyAsync(string srcPath, string destPath);` - copy file in storage
1. `Task MoveAsync(string srcPath, string destPath);` - move file in storage
1. `Task DeleteAsync(string pathToFile);` - delete file from storage
1. `Task<bool> Exist(string path);` - check exist file in storage
1. `Task<FileInfo> UploadFileAsync(Stream stream,string pathToFile);` - Upload file to storage
1. `Task<DownloadResult> DownloadFileAsync(string pathToFile);` - download file from storage

### Repository implementation

1. [GirdFS](Skidbladnir.Storage.GridFS/README.md) - Implementation of file storage abstraction based on GridFS (file storage in mongodb)
2. [LocalFs](Skidbladnir.Storage.LocalFileStorage/README.md) - Implementing a file storage abstraction on the file system of the host that the application is running on
