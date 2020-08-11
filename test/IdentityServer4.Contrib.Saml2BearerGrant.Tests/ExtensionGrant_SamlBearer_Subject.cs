using FluentAssertions;
using IdentityServer4.Contrib.Saml2BearerGrant.Extensions;
using IdentityServer4.Contrib.Saml2BearerGrant.Saml;
using IdentityServer4.Contrib.Saml2BearerGrant.Saml2;
using IdentityServer4.Contrib.Saml2BearerGrant.Tests.Helpers;
using System.Threading.Tasks;
using System.Xml;
using Xunit;

namespace IdentityServer4.Contrib.Saml2BearerGrant.Tests.Saml2
{
    public class ExtensionGrant_SamlBearer_Subject : IClassFixture<SamlBearerGrantValidatorFactory>
    {
        private readonly SamlBearerGrantValidatorFactory _factory;

        public ExtensionGrant_SamlBearer_Subject(SamlBearerGrantValidatorFactory factory)
        {
            _factory = factory;
        }

        [Fact] public async Task saml_missing_subject() => await missing_subject<SamlBearerGrantValidator>();
        [Fact] public async Task saml2_missing_subject() => await missing_subject<Saml2BearerGrantValidator>();

        private async Task missing_subject<T>() where T : BaseSamlBearerGrantValidator
        {
            var validator = _factory.CreateValidator<T>();
            var context = new DefaultExtensionGrantValidationContext();

            string assertion = _factory.CreateTokenWithTestDefaults<T>();
            assertion = RemoveSubjectElement(assertion);
            context.Request.Raw["assertion"] = assertion.ToBase64Url();

            await validator.ValidateAsync(context);

            context.Result.Error.Should().NotBeNull();
            context.Result.Subject.Should().BeNull();
        }

        private string RemoveSubjectElement(string assertionXml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(assertionXml);

            XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("saml", doc.DocumentElement.GetAttribute("xmlns:saml"));

            var subject = doc.SelectSingleNode("//saml:Subject", ns);
            var parent = subject.ParentNode;
            parent.RemoveChild(subject);
            return doc.OuterXml;
        }
    }
}
