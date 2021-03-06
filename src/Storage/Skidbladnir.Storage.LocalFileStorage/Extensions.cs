using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Skidbladnir.Storage.Abstractions;
using FileInfo = Skidbladnir.Storage.Abstractions.FileInfo;

namespace Skidbladnir.Storage.LocalFileStorage
{
    public static class Extensions
    {
        public static IServiceCollection AddLocalFsStorage(this IServiceCollection services, string storagePath)
        {
            var storageInfo = new LocalStorageInfo(storagePath);
            services.AddSingleton<LocalStorageInfo>(storageInfo);
            services.AddSingleton<IStorage<LocalStorageInfo>, LocalStorage<LocalStorageInfo>>();
            return services;
        }

        public static IServiceCollection AddLocalFsStorage<TStorageInfo>(this IServiceCollection services, string name,
            string storagePath)
            where TStorageInfo : LocalStorageInfo
        {
            var infoType = typeof(TStorageInfo);
            var constructorInfo = infoType.GetConstructor(new[] {typeof(string), typeof(string)});
            if (constructorInfo == null)
                throw new InvalidOperationException("Can't find constructor with 2 string input params");

            var storageInfo = (TStorageInfo) constructorInfo.Invoke(new[] {name, storagePath});
            services.AddSingleton<TStorageInfo>(storageInfo);
            services.AddSingleton<IStorage<TStorageInfo>, LocalStorage<TStorageInfo>>();
            return services;
        }

        internal static string GetPathWithoutFileName(this string path)
        {
            var splitedPath = path.Split(Path.DirectorySeparatorChar);
            if (splitedPath.Length <= 0)
                return path;

            return string.Join(Path.DirectorySeparatorChar.ToString(),
                splitedPath.Take(splitedPath.Length - 1).ToArray());
        }

        internal static FileInfo ToFileInfo(this System.IO.FileInfo info, LocalStorageInfo storageInfo)
        {
            var relaivePath = info.FullName.Replace(storageInfo.StoragePath, "").EscapePath().StripPath();
            return new FileInfo(relaivePath, info.Length, info.CreationTimeUtc);
        }
    }
}