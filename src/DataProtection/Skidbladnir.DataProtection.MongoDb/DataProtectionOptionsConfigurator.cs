using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.Options;

namespace Skidbladnir.DataProtection.MongoDb
{
    public class DataProtectionOptionsConfigurator : IConfigureOptions<KeyManagementOptions>
    {
        private readonly IXmlRepository _repository;

        public DataProtectionOptionsConfigurator(IXmlRepository repository)
        {
            _repository = repository;
        }

        public void Configure(KeyManagementOptions options)
        {
            options.XmlRepository = _repository;
        }
    }

}