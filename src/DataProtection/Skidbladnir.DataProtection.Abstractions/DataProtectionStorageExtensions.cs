using System.Xml.Linq;

namespace Skidbladnir.DataProtection.Abstractions
{
    internal static class DataProtectionStorageExtensions
    {
        private const string IdAttribute = "id";

        internal static DbXmlKey ToNewDbXmlKey(this XElement element)
        {
            var key = element.ToString(SaveOptions.DisableFormatting);
            var keyId = element.Attribute(IdAttribute)?.Value;
            return new DbXmlKey(string.Empty, keyId, key);
        }

        internal static XElement ToKeyXmlElement(this DbXmlKey dbKey)
        {
            return XElement.Parse(dbKey.Key);
        }
    }

}