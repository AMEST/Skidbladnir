using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Skidbladnir.Storage.Abstractions;
using WebDav;
using FileInfo = Skidbladnir.Storage.Abstractions.FileInfo;

namespace Skidbladnir.Storage.WebDav
{
    public static class Extensions
    {
        public static IServiceCollection AddWebDavStorage(this IServiceCollection services, WebDavStorageConfiguration storageConfiguration)
        {
            var storageInfo = new WebDavStorageInfo(storageConfiguration);
            services.AddSingleton<WebDavStorageInfo>(storageInfo);
            services.AddSingleton<IStorage<WebDavStorageInfo>, WebDavStorage<WebDavStorageInfo>>();
            return services;
        }

        public static IServiceCollection AddWebDavStorage<TStorageInfo>(this IServiceCollection services, string name,
            WebDavStorageConfiguration storageConfiguration)
            where TStorageInfo : WebDavStorageInfo
        {
            var infoType = typeof(TStorageInfo);
            var constructorInfo = infoType.GetConstructor(new[] { typeof(string), typeof(WebDavStorageConfiguration) });
            if (constructorInfo == null)
                throw new InvalidOperationException("Can't find constructor with 2 input params");

            var storageInfo = (TStorageInfo)constructorInfo.Invoke(new object[] { name, storageConfiguration });
            services.AddSingleton<TStorageInfo>(storageInfo);
            services.AddSingleton<IStorage<TStorageInfo>, WebDavStorage<TStorageInfo>>();
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

        internal static FileInfo ToFileInfo(this WebDavResource resource, string pathToFile)
        {
            return new FileInfo(pathToFile, resource.ContentLength ?? 0, resource.LastModifiedDate ?? DateTime.MinValue);
        }
    }
}