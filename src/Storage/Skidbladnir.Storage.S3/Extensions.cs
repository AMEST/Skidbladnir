using System;
using Microsoft.Extensions.DependencyInjection;
using Skidbladnir.Storage.Abstractions;

namespace Skidbladnir.Storage.S3
{
    public static class Extensions
    {
        public static IServiceCollection AddGridFsStorage(this IServiceCollection services, S3StorageConfiguration configuration)
        {
            var storageInfo = new S3StorageInfo(configuration.ServiceUrl, configuration.AccessKey, configuration.SecretKey, configuration.Bucket);
            services.AddSingleton<S3StorageInfo>(storageInfo);
            services.AddSingleton<IStorage<S3StorageInfo>, S3Storage<S3StorageInfo>>();
            return services;
        }

        public static IServiceCollection AddGridFsStorage<TStorageInfo>(this IServiceCollection services, string name, S3StorageInfo configuration)
            where TStorageInfo : S3StorageInfo
        {
            var infoType = typeof(TStorageInfo);
            var constructorInfo = infoType.GetConstructor(new[] { typeof(string), typeof(string), typeof(string), typeof(string), typeof(string) });
            if (constructorInfo == null)
                throw new InvalidOperationException("Can't find constructor with 5 string input params");

            var storageInfo = (TStorageInfo)constructorInfo.Invoke(new[] { name, configuration.ServiceUrl, configuration.AccessKey, configuration.SecretKey, configuration.Bucket });
            services.AddSingleton<TStorageInfo>(storageInfo);
            services.AddSingleton<IStorage<TStorageInfo>, S3Storage<TStorageInfo>>();
            return services;
        }

        internal static string NormalizePath(this string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return "/";

            var normalizedPath = path.Replace("\\", "/").Replace("//", "/");
            return path.StartsWith("/")
                ? normalizedPath
                : $"/{normalizedPath}";
        }
    }
}