namespace IdentityServer4.Contrib.Saml2BearerGrant
{
    /// <summary>
    /// Log messages copied from Microsoft.IdentityModel.Tokens.LogMessages class
    /// because it is an internal class.
    /// </summary>
    internal static class LogMessages
    {
        public const string IDX10222 = "IDX10222: Lifetime validation failed. The token is not yet valid. ValidFrom: '{0}', Current time: '{1}'.";
        public const string IDX10223 = "IDX10223: Lifetime validation failed. The token is expired. ValidTo: '{0}', Current time: '{1}'.";
        public const string IDX10224 = "IDX10224: Lifetime validation failed. The NotBefore: '{0}' is after Expires: '{1}'.";
        public const string IDX10225 = "IDX10225: Lifetime validation failed. The token is missing an Expiration Time. Tokentype: '{0}'.";
        public const string IDX10238 = "IDX10238: ValidateLifetime property on ValidationParameters is set to false. Exiting without validating the lifetime.";
        public const string IDX10239 = "IDX10239: Lifetime of the token is valid.";
    }
}
