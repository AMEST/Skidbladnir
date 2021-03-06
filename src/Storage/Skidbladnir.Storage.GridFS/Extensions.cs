using System;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver.GridFS;
using Skidbladnir.Storage.Abstractions;

namespace Skidbladnir.Storage.GridFS
{
    public static class Extensions
    {
        public static IServiceCollection AddGridFsStorage(this IServiceCollection services, string connectionString)
        {
            var storageInfo = new GridFsStorageInfo(connectionString);
            services.AddSingleton<GridFsStorageInfo>(storageInfo);
            services.AddSingleton<IStorage<GridFsStorageInfo>, GridFsStorage<GridFsStorageInfo>>();
            return services;
        }

        public static IServiceCollection AddGridFsStorage<TStorageInfo>(this IServiceCollection services, string name,
            string connectionString)
            where TStorageInfo : GridFsStorageInfo
        {
            var infoType = typeof(TStorageInfo);
            var constructorInfo = infoType.GetConstructor(new[] { typeof(string), typeof(string) });
            if (constructorInfo == null)
                throw new InvalidOperationException("Can't find constructor with 2 string input params");

            var storageInfo = (TStorageInfo)constructorInfo.Invoke(new[] { name, connectionString });
            services.AddSingleton<TStorageInfo>(storageInfo);
            services.AddSingleton<IStorage<TStorageInfo>, GridFsStorage<TStorageInfo>>();
            return services;
        }

        internal static string NormilizePath(this string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return "/";

            var normilizedPath = path.Replace("\\", "/");
            return path.StartsWith("/")
                ? normilizedPath
                : $"/{normilizedPath}";
        }

        internal static FileInfo ToFileInfo(this GridFSFileInfo info)
        {
            return new FileInfo(info.Filename, info.Length, info.UploadDateTime);
        }
    }
}