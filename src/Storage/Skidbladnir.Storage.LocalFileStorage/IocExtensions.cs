using System;
using Microsoft.Extensions.DependencyInjection;
using Skidbladnir.Storage.Abstractions;

namespace Skidbladnir.Storage.LocalFileStorage
{
    public static class IocExtensions
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
            if(constructorInfo == null)
                throw new InvalidOperationException("Can't find constructor with 2 string input params");

            var storageInfo = (TStorageInfo) constructorInfo.Invoke(new[] { name, storagePath });
            services.AddSingleton<TStorageInfo>(storageInfo);
            services.AddSingleton<IStorage<TStorageInfo>, LocalStorage<TStorageInfo>>();
            return services;
        }
    }
}