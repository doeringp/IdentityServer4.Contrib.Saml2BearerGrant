using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer4.Contrib.Saml2BearerGrant.Saml2
{
    public class Saml2BearerGrantValidator : BaseSamlBearerGrantValidator
    {
        public override string GrantType => "urn:ietf:params:oauth:grant-type:saml2-bearer";

        public override SecurityTokenHandler TokenHandler => new Saml2BearerGrantSecurityTokenHandler();

        public Saml2BearerGrantValidator(IKeyMaterialService keys,
                                         IHttpContextAccessor httpContextAccessor,
                                         ILogger<Saml2BearerGrantValidator> logger,
                                         ISystemClock systemClock)
                                     : base(keys, httpContextAccessor, logger, systemClock)
        {
        }
    }
}
