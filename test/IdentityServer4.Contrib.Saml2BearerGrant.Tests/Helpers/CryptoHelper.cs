using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace IdentityServer4.Contrib.Saml2BearerGrant.Tests.Helpers
{
    public static class CryptoHelper
    {
        public static SigningCredentials GenerateSigningCredentials()
        {
            const string signingAlgorithm = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
            const string digestAlgorithm = "http://www.w3.org/2001/04/xmlenc#sha256";
            var rsaKey = CreateRsaSecurityKey();
            return new SigningCredentials(rsaKey, signingAlgorithm, digestAlgorithm);
        }

        /// <summary>
        /// Creates a new RSA security key.
        /// </summary>
        /// <returns></returns>
        public static RsaSecurityKey CreateRsaSecurityKey()
        {
            var rsa = RSA.Create();
            RsaSecurityKey key;

            if (rsa is RSACryptoServiceProvider)
            {
                rsa.Dispose();
                var cng = new RSACng(2048);

                var parameters = cng.ExportParameters(includePrivateParameters: true);
                key = new RsaSecurityKey(parameters);
            }
            else
            {
                rsa.KeySize = 2048;
                key = new RsaSecurityKey(rsa);
            }

            key.KeyId = CryptoRandom.CreateUniqueId(16);
            return key;
        }
    }
}
