using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace IdentityServer4.Contrib.Saml2BearerGrant.Tests.Helpers
{
    public static class SecurityTokenFactory
    {
        public static string CreateTokenWithTestDefaults<T>(this SamlBearerGrantValidatorFactory factory,
                                                            SecurityTokenDescriptor options = null) where T: BaseSamlBearerGrantValidator
        {
            SecurityTokenDescriptor tokenDescriptor = factory.CreateDefaultTokenDescriptor();

            tokenDescriptor.Audience = options?.Audience ?? tokenDescriptor.Audience;
            tokenDescriptor.NotBefore = options?.NotBefore ?? tokenDescriptor.NotBefore;
            tokenDescriptor.Expires = options?.Expires ?? tokenDescriptor.Expires;
            tokenDescriptor.Issuer = options?.Issuer ?? tokenDescriptor.Issuer;
            tokenDescriptor.SigningCredentials = options?.SigningCredentials ?? tokenDescriptor.SigningCredentials;
            tokenDescriptor.Subject = options?.Subject ?? tokenDescriptor.Subject;

            return factory.CreateToken<T>(tokenDescriptor);
        }

        public static string CreateToken<T>(this SamlBearerGrantValidatorFactory factory,
                                            SecurityTokenDescriptor tokenDescriptor) where T : BaseSamlBearerGrantValidator
        {
            var validator = factory.CreateValidator<T>();
            SecurityToken token = validator.TokenHandler.CreateToken(tokenDescriptor);
            return validator.TokenHandler.ToXml(token);
        }

        public static SecurityTokenDescriptor CreateDefaultTokenDescriptor(this SamlBearerGrantValidatorFactory factory)
        {
            return new SecurityTokenDescriptor
            {
                Audience = Default.RelyingParty,
                NotBefore = Default.NotBefore,
                Expires = Default.Expires,
                Issuer = Default.Issuer,
                SigningCredentials = factory.SigningCredentials,
                Subject = new ClaimsIdentity(Default.SamlClaims)
            };
        }
    }
}
