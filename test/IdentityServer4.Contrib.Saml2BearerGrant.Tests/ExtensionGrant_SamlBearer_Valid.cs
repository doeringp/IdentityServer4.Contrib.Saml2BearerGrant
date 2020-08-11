using FluentAssertions;
using IdentityServer4.Contrib.Saml2BearerGrant.Extensions;
using IdentityServer4.Contrib.Saml2BearerGrant.Saml;
using IdentityServer4.Contrib.Saml2BearerGrant.Saml2;
using IdentityServer4.Contrib.Saml2BearerGrant.Tests.Helpers;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace IdentityServer4.Contrib.Saml2BearerGrant.Tests.Saml2
{
    public class ExtensionGrant_SamlBearer_Valid : IClassFixture<SamlBearerGrantValidatorFactory>
    {
        private readonly SamlBearerGrantValidatorFactory _factory;
        private readonly ITestOutputHelper _output;

        public ExtensionGrant_SamlBearer_Valid(SamlBearerGrantValidatorFactory factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
        }

        [Fact] public async Task saml_valid_assertion() => await valid_assertion<SamlBearerGrantValidator>();
        [Fact] public async Task saml2_valid_assertion() => await valid_assertion<Saml2BearerGrantValidator>();

        private async Task valid_assertion<T>() where T : BaseSamlBearerGrantValidator
        {
            var validator = _factory.CreateValidator<T>();
            var context = new DefaultExtensionGrantValidationContext();
            string assertion = _factory.CreateTokenWithTestDefaults<T>();
            _output.WriteLine(assertion.ToPrettyXml());
            context.Request.Raw["assertion"] = assertion.ToBase64Url();

            await validator.ValidateAsync(context);

            context.Result.Error.Should().BeNull();
            context.Result.Subject.Should().NotBeNull();
        }
    }
}
