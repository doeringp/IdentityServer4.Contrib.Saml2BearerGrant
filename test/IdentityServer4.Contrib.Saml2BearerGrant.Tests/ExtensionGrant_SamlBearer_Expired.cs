using FluentAssertions;
using IdentityServer4.Contrib.Saml2BearerGrant.Extensions;
using IdentityServer4.Contrib.Saml2BearerGrant.Saml;
using IdentityServer4.Contrib.Saml2BearerGrant.Saml2;
using IdentityServer4.Contrib.Saml2BearerGrant.Tests.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer4.Contrib.Saml2BearerGrant.Tests.Saml2
{
    public class ExtensionGrant_SamlBearer_Expired : IClassFixture<SamlBearerGrantValidatorFactory>
    {
        private readonly SamlBearerGrantValidatorFactory _factory;

        public ExtensionGrant_SamlBearer_Expired(SamlBearerGrantValidatorFactory factory)
        {
            _factory = factory;
        }

        [Fact] public async Task saml_expired_assertion() => await expired_assertion<SamlBearerGrantValidator>();
        [Fact] public async Task saml2_expired_assertion() => await expired_assertion<Saml2BearerGrantValidator>();

        private async Task expired_assertion<T>() where T : BaseSamlBearerGrantValidator
        {
            var validator = _factory.CreateValidator<T>();
            var context = new DefaultExtensionGrantValidationContext();

            string assertion = _factory.CreateTokenWithTestDefaults<T>(new SecurityTokenDescriptor
            {
                Expires = Default.ExpiresInPast
            });
            context.Request.Raw["assertion"] = assertion.ToBase64Url();

            await validator.ValidateAsync(context);

            context.Result.Error.Should().Be("invalid_grant");
            context.Result.Subject.Should().BeNull();
        }
    }
}
