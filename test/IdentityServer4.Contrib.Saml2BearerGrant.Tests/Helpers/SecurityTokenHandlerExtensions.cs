using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Text;
using System.Xml;

namespace IdentityServer4.Contrib.Saml2BearerGrant.Tests.Helpers
{
    public static class SecurityTokenHandlerExtensions
    {
        /// <summary>
        /// Serialize a SecurityToken to xml.
        /// </summary>
        public static string ToXml(this SecurityTokenHandler tokenHandler, SecurityToken token)
        {
            var sb = new StringBuilder();
            using (var xmlWriter = new XmlTextWriter(new StringWriter(sb)))
            {
                tokenHandler.WriteToken(xmlWriter, token);
                return sb.ToString();
            }
        }
    }
}
