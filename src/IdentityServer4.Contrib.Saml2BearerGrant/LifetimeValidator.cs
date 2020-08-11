using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;

namespace IdentityServer4.Contrib.Saml2BearerGrant
{
    internal class LifetimeValidator
    {
        private readonly ISystemClock _systemClock;

        public LifetimeValidator(ISystemClock systemClock)
        {
            _systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
        }

        /// <summary>
        /// Validates the lifetime of a <see cref="SecurityToken"/>.
        /// </summary>
        /// <remarks>
        /// I had to write a custom lifetime validator because the original validator
        /// didn't support changing the current time for testing.
        /// They're using a hardcoded DateTime.UtcNow.
        /// </remarks>
        public bool ValidateLifetime(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (validationParameters == null)
                throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            if (!validationParameters.ValidateLifetime)
            {
                LogHelper.LogInformation(LogMessages.IDX10238);
                return true;
            }

            if (!expires.HasValue && validationParameters.RequireExpirationTime)
                throw LogHelper.LogExceptionMessage(new SecurityTokenNoExpirationException(LogHelper.FormatInvariant(LogMessages.IDX10225, securityToken == null ? "null" : securityToken.GetType().ToString())));

            if (notBefore.HasValue && expires.HasValue && (notBefore.Value > expires.Value))
                throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidLifetimeException(LogHelper.FormatInvariant(LogMessages.IDX10224, notBefore.Value, expires.Value))
                { NotBefore = notBefore, Expires = expires });

            // XXX: Changed DateTime.UtcNow to ISystemClock.UtcNow.DateTIme.
            DateTime utcNow = _systemClock.UtcNow.DateTime;
            if (notBefore.HasValue && (notBefore.Value > DateTimeUtil.Add(utcNow, validationParameters.ClockSkew)))
                throw LogHelper.LogExceptionMessage(new SecurityTokenNotYetValidException(LogHelper.FormatInvariant(LogMessages.IDX10222, notBefore.Value, utcNow))
                { NotBefore = notBefore.Value });

            if (expires.HasValue && (expires.Value < DateTimeUtil.Add(utcNow, validationParameters.ClockSkew.Negate())))
                throw LogHelper.LogExceptionMessage(new SecurityTokenExpiredException(LogHelper.FormatInvariant(LogMessages.IDX10223, expires.Value, utcNow))
                { Expires = expires.Value });

            // if it reaches here, that means lifetime of the token is valid
            LogHelper.LogInformation(LogMessages.IDX10239);

            return true;
        }
    }
}
