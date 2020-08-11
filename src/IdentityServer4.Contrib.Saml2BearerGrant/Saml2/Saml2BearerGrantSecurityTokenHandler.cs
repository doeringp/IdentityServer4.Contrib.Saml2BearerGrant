using IdentityServer4.Contrib.Saml2BearerGrant.Exceptions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml2;

namespace IdentityServer4.Contrib.Saml2BearerGrant.Saml2
{
    public class Saml2BearerGrantSecurityTokenHandler : Saml2SecurityTokenHandler
    {
        protected override void ValidateSubject(Saml2SecurityToken samlToken, TokenValidationParameters validationParameters)
        {
            base.ValidateSubject(samlToken, validationParameters);

            foreach (var subjectConfirmation in samlToken.Assertion.Subject.SubjectConfirmations)
            {
                const string expectedMethod = "urn:oasis:names:tc:SAML:2.0:cm:bearer";

                if (!subjectConfirmation.Method.Equals(expectedMethod))
                {
                    throw new SecurityTokenInvalidSubjectConfirmationException(
                        "The assertion must have a <Subject> element which contains at least one <SubjectConfirmation> " +
                        $"element with an attribute \"Method\" with a value of \"{expectedMethod}\".");
                }
            }
        }
    }
}
