using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Skidbladnir.Repository.Abstractions;

namespace Skidbladnir.DataProtection.Abstractions
{
    public class XmlRepository : IXmlRepository
    {
        private readonly IRepository<DbXmlKey> _keys;

        public XmlRepository(IRepository<DbXmlKey> keys)
        {
            _keys = keys;
        }

        public IReadOnlyCollection<XElement> GetAllElements()
        {
            var keys = _keys.GetAll().ToList();
            return keys.Select(key => key.ToKeyXmlElement()).ToList();
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            _keys.Create(element.ToNewDbXmlKey()).GetAwaiter().GetResult();
        }
    }
}