using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Skidbladnir.Modules
{
    public class ModulesConfiguration
    {
        private readonly ConcurrentDictionary<Type, object> _modulesConfiguration;
        public ModulesConfiguration(IConfiguration configuration)
        {
            AppConfiguration = configuration;
            _modulesConfiguration = new ConcurrentDictionary<Type, object>();
        }

        /// <summary>
        /// Microsoft Extensions Configuration
        /// </summary>
        public IConfiguration AppConfiguration { get; }

        /// <summary>
        /// Get configuration from store
        /// </summary>
        public T Get<T>() where T : class
        {
            if (_modulesConfiguration.TryGetValue(typeof(T), out var config))
                return config as T;

            throw new ArgumentException($"Configuration with type `{typeof(T).Name}` not found");
        }

        /// <summary>
        /// Get configuration from store or return default value
        /// </summary>
        public T Get<T>(T defaultValue) where T : class
        {
            if (_modulesConfiguration.TryGetValue(typeof(T), out var config))
                return config as T;

            return defaultValue;
        }

        /// <summary>
        /// Get configuration from store or create configuration and bind from section in Microsoft.Extensions.Configuration 
        /// </summary>
        public T GetOrCreate<T>(string section) where T : class, new()
        {
            if (_modulesConfiguration.TryGetValue(typeof(T), out var config))
                return config as T;

            var typedConfig = new T();
            AppConfiguration.GetSection(section).Bind(typedConfig);

            AddOrReplace(typedConfig);
            return typedConfig;
        }

        public void Add<T>(T config) where T : class
        {
            AddOrReplace(config);
        }

        private void AddOrReplace<T>(T config)
        {
            var configType = typeof(T);
            var allTypes = new List<Type>(configType.GetInterfaces()) {configType};
            foreach (var type in allTypes)
            {
                _modulesConfiguration.AddOrUpdate(type, config, (t, o) => config);
            }
        }
    }
}