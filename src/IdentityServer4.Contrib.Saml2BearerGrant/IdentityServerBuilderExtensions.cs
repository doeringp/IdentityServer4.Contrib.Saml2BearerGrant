using IdentityServer4.Contrib.Saml2BearerGrant.Saml;
using IdentityServer4.Contrib.Saml2BearerGrant.Saml2;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityServerBuilderExtensions
    {
        /// <summary>
        /// Adds the extension grant type "urn:ietf:params:oauth:grant-type:saml2-bearer" (RFC 7522)
        /// for the Token Endpoint to IdentityServer.
        /// 
        /// The extension has the limitation that you can use only SAML2 assertions that have been issued
        /// by IdentityServer itself.
        /// 
        /// Furthermore the client id and secret are required for each token request.
        /// </summary>
        public static IIdentityServerBuilder AddSaml2BearerGrant(this IIdentityServerBuilder builder)
        {
            return builder.AddExtensionGrantValidator<Saml2BearerGrantValidator>();
        }

        /// <summary>
        /// Adds the extension grant type "urn:ietf:params:oauth:grant-type:saml-bearer"
        /// for the Token Endpoint to IdentityServer.
        /// 
        /// The extension has the limitation that you can use only SAML2 assertions that have been issued
        /// by IdentityServer itself.
        /// 
        /// Furthermore the client id and secret are required for each token request.
        /// </summary>
        public static IIdentityServerBuilder AddSamlBearerGrant(this IIdentityServerBuilder builder)
        {
            return builder.AddExtensionGrantValidator<SamlBearerGrantValidator>();
        }
    }
}
