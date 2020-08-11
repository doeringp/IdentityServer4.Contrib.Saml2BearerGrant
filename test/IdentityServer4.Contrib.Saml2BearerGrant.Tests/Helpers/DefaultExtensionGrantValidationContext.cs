using IdentityServer4.Validation;
using System.Collections.Specialized;

namespace IdentityServer4.Contrib.Saml2BearerGrant.Tests.Helpers
{
    public class DefaultExtensionGrantValidationContext : ExtensionGrantValidationContext
    {
        public DefaultExtensionGrantValidationContext()
        {
            Request = new ValidatedTokenRequest();
            Request.Raw = new NameValueCollection();
            Request.Client = new Models.Client
            {
                ClientId = Default.RelyingParty
            };
        }
    }
}
