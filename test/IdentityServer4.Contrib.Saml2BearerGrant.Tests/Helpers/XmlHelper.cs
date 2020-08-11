using System.IO;
using System.Text;
using System.Xml;

namespace IdentityServer4.Contrib.Saml2BearerGrant.Tests.Helpers
{
    public static class XmlHelper
    {
        public static string ToPrettyXml(this string xml)
        {
            if (xml == null)
            {
                return null;
            }
            var memoryStream = new MemoryStream();
            using (var writer = new XmlTextWriter(memoryStream, Encoding.Unicode))
            {
                writer.Formatting = Formatting.Indented;
                var document = new XmlDocument();
                document.LoadXml(xml);
                document.WriteContentTo(writer);
                writer.Flush();
                memoryStream.Flush();
                memoryStream.Position = 0;
                return new StreamReader(memoryStream).ReadToEnd();
            }
        }
    }
}
