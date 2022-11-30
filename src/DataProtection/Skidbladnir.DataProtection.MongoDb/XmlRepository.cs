using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection.Repositories;
using MongoDB.Driver;

namespace Skidbladnir.DataProtection.MongoDb
{
    internal class XmlRepository : IXmlRepository
    {
        private readonly MongoDbContext _context;

        public XmlRepository(MongoDbContext context)
        {
            _context = context;
        }

        internal IMongoCollection<DbXmlKey> Collection => _context.GetCollection();

        public IReadOnlyCollection<XElement> GetAllElements()
        {
            return Collection.AsQueryable()
                    .ToList()
                    .Select(x => x.ToKeyXmlElement())
                    .ToList();
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            Collection.InsertOne(element.ToNewDbXmlKey());
        }
    }
}