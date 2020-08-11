using IdentityModel;
using System.Text;

namespace IdentityServer4.Contrib.Saml2BearerGrant.Extensions
{
    public static class Base64UrlStringExtensions
    {
        private static readonly Encoding Encoding = Encoding.UTF8;

        public static string ToBase64Url(this string text)
        {
            byte[] bytes = Encoding.GetBytes(text);
            return Base64Url.Encode(bytes);
        }

        public static string FromBase64Url(this string base64UrlValue)
        {
            byte[] bytes = Base64Url.Decode(base64UrlValue);
            return Encoding.GetString(bytes);
        }
    }
}
