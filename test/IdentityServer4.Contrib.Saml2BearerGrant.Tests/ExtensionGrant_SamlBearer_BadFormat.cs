using FluentAssertions;
using IdentityServer4.Contrib.Saml2BearerGrant.Extensions;
using IdentityServer4.Contrib.Saml2BearerGrant.Saml;
using IdentityServer4.Contrib.Saml2BearerGrant.Saml2;
using IdentityServer4.Contrib.Saml2BearerGrant.Tests.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer4.Contrib.Saml2BearerGrant.Tests.Saml2
{
    public class ExtensionGrant_SamlBearer_BadFormat : IClassFixture<SamlBearerGrantValidatorFactory>
    {
        private readonly SamlBearerGrantValidatorFactory _factory;

        public ExtensionGrant_SamlBearer_BadFormat(SamlBearerGrantValidatorFactory factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("foobarblub")]
        [InlineData(":fooba%rblub!")]
        [InlineData("<foo><bar>xxx</bar></foo>")]
        // deflated and base64url encoded SAML assertion (we do not support deflated SAML).
        [InlineData("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")]
        public async Task saml_invalid_xml_format(string assertion) =>
            await invalid_xml_format<SamlBearerGrantValidator>(assertion);

        [Theory]
        [InlineData("foobarblub")]
        [InlineData(":fooba%rblub!")]
        [InlineData("<foo><bar>xxx</bar></foo>")]
        // deflated and base64url encoded SAML assertion (we do not support deflated SAML).
        [InlineData("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")]
        public async Task saml2_invalid_xml_format(string assertion) =>
            await invalid_xml_format<Saml2BearerGrantValidator>(assertion);

        private async Task invalid_xml_format<T>(string assertion) where T : BaseSamlBearerGrantValidator
        {
            var validator = _factory.CreateValidator<T>();
            var context = new DefaultExtensionGrantValidationContext();
            context.Request.Raw["assertion"] = assertion.ToBase64Url();

            await validator.ValidateAsync(context);

            context.Result.Error.Should().Be("invalid_grant");
            context.Result.Subject.Should().BeNull();
        }

        [Theory]
        [InlineData("sasfjslDSs=")]
        [InlineData("foobarblub")]
        [InlineData("<foo><bar>xxx</bar></foo>")]
        public async Task saml_no_base64url_format(string assertion) => 
            await no_base64url_format<SamlBearerGrantValidator>(assertion);

        [Theory]
        [InlineData("sasfjslDSs=")]
        [InlineData("foobarblub")]
        [InlineData("<foo><bar>xxx</bar></foo>")]
        public async Task saml2_no_base64url_format(string assertion) => 
            await no_base64url_format<Saml2BearerGrantValidator>(assertion);

        private async Task no_base64url_format<T>(string assertion) where T : BaseSamlBearerGrantValidator
        {
            var validator = _factory.CreateValidator<T>();
            var context = new DefaultExtensionGrantValidationContext();
            context.Request.Raw["assertion"] = assertion;

            await validator.ValidateAsync(context);

            context.Result.Error.Should().Be("invalid_grant");
            context.Result.Subject.Should().BeNull();
        }


        [Fact] public async Task saml_missing_assertion_param() => await missing_assertion_param<SamlBearerGrantValidator>();
        [Fact] public async Task saml2_missing_assertion_param() => await missing_assertion_param<Saml2BearerGrantValidator>();

        private async Task missing_assertion_param<T>() where T : BaseSamlBearerGrantValidator
        {
            var validator = _factory.CreateValidator<T>();
            var context = new DefaultExtensionGrantValidationContext();

            await validator.ValidateAsync(context);

            context.Result.Error.Should().Be("invalid_grant");
            context.Result.Subject.Should().BeNull();
        }
    }
}
