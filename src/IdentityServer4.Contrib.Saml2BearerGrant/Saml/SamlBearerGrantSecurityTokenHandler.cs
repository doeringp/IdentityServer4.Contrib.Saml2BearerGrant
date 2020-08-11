using IdentityServer4.Contrib.Saml2BearerGrant.Exceptions;
using Microsoft.IdentityModel.Tokens.Saml;
using System.Linq;
using System.Security.Claims;

namespace IdentityServer4.Contrib.Saml2BearerGrant.Saml
{
    public class SamlBearerGrantSecurityTokenHandler : SamlSecurityTokenHandler
    {
        protected override void ProcessSubject(SamlSubject subject, ClaimsIdentity identity, string issuer)
        {
            base.ProcessSubject(subject, identity, issuer);

            const string expectedMethod = "urn:oasis:names:tc:SAML:1.0:cm:bearer";

            if (!subject.ConfirmationMethods.Any(method => method.Equals(expectedMethod)))
            {
                throw new SecurityTokenInvalidSubjectConfirmationException(
                        "The assertion must have a <Subject> element which contains at least one <SubjectConfirmation> " +
                        $"element with a <ConfirmationMethod> element with a value of \"{expectedMethod}\".");
            }
        }
    }
}
