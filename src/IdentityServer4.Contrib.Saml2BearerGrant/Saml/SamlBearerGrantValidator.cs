using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer4.Contrib.Saml2BearerGrant.Saml
{
    public class SamlBearerGrantValidator : BaseSamlBearerGrantValidator
    {
        public override string GrantType => "urn:ietf:params:oauth:grant-type:saml-bearer";

        public override SecurityTokenHandler TokenHandler => new SamlBearerGrantSecurityTokenHandler();

        public SamlBearerGrantValidator(IKeyMaterialService keys,
                                        IHttpContextAccessor httpContextAccessor,
                                        ILogger<SamlBearerGrantValidator> logger,
                                        ISystemClock systemClock)
                                     : base(keys, httpContextAccessor, logger, systemClock)
        {
        }
    }
}
