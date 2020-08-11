using Microsoft.IdentityModel.Tokens;

namespace IdentityServer4.Contrib.Saml2BearerGrant.Exceptions
{
    /// <summary>
    /// This exception is thrown when the &lt;Subject&gt; element has no &lt;SubjectConfirmation&gt;
    /// element with an attribute "Method" with a value of "urn:oasis:names:tc:SAML:2.0:cm:bearer".
    /// </summary>
    public class SecurityTokenInvalidSubjectConfirmationException : SecurityTokenValidationException
    {
        public SecurityTokenInvalidSubjectConfirmationException(string message) : base(message)
        {
        }
    }
}
