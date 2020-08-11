using IdentityServer4.Contrib.Saml2BearerGrant.Exceptions;
using IdentityServer4.Contrib.Saml2BearerGrant.Extensions;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml;
using Microsoft.IdentityModel.Tokens.Saml2;
using Microsoft.IdentityModel.Xml;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.Contrib.Saml2BearerGrant
{
    public abstract class BaseSamlBearerGrantValidator : IExtensionGrantValidator
    {
        public const string NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        public abstract string GrantType { get; }

        public abstract SecurityTokenHandler TokenHandler { get; }

        private readonly IKeyMaterialService _keys;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private readonly ISystemClock _systemClock;

        protected BaseSamlBearerGrantValidator(IKeyMaterialService keys,
                                            IHttpContextAccessor httpContextAccessor, 
                                            ILogger logger, 
                                            ISystemClock systemClock)
        {
            _keys = keys ?? throw new ArgumentNullException(nameof(keys));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentException(nameof(httpContextAccessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
        }

        public virtual async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var tokenHandler = TokenHandler;

            if (tokenHandler == null)
                throw new Exception($"The property {nameof(TokenHandler)} may not be null!");

            var rawAssertion = context.Request.Raw.Get("assertion");

            if (string.IsNullOrEmpty(rawAssertion))
            {
                _logger.LogWarning("SAML response not found in assertion query string param.");
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant)
                { ErrorDescription = "Parameter 'assertion' is missing." };
                return;
            }

            var client = context.Request.Client;
            if (client == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient);
                return;
            }

            string xml = null;
            try
            {
                xml = rawAssertion.FromBase64Url();
            }
            catch (Exception ex)
            {
                _logger.LogWarning("The parameter \"assertion\" can not be parsed. {message}", ex.Message);
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant)
                { ErrorDescription = "Illegal base64url string" };
                return;
            }

            IEnumerable<SecurityKey> signingKeys = await _keys.GetValidationKeysAsync();
            string idSrvIssuerUri = _httpContextAccessor.HttpContext.GetIdentityServerIssuerUri();
            var lifetimeValidator = new LifetimeValidator(_systemClock);

            var tokenValidationParams = new TokenValidationParameters
            {
                NameClaimType = NameClaimType,
                RequireSignedTokens = true,
                IssuerSigningKeys = signingKeys,
                ValidIssuer = idSrvIssuerUri,
                ValidateAudience = false,
                LifetimeValidator = lifetimeValidator.ValidateLifetime
            };

            ClaimsPrincipal principal = null;
            try
            {
                principal = tokenHandler.ValidateToken(
                    xml, tokenValidationParams, out SecurityToken validatedToken);
            }
            catch (Exception ex) when (ex is System.Xml.XmlException || ex is XmlReadException)
            {
                _logger.LogWarning("The SAML assertion was no valid xml: {message}", ex.Message);
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant)
                    { ErrorDescription = "Invalid xml format" };
                return;
            }
            catch (Exception ex) when (ex is Saml2SecurityTokenReadException || ex is SamlSecurityTokenReadException)
            {
                _logger.LogWarning("Invalid saml format: {message}", ex.Message);
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant)
                    { ErrorDescription = "Invalid saml format" };
                return;
            }
            catch (SecurityTokenInvalidSubjectConfirmationException ex)
            {
                _logger.LogWarning("Invalid subject confirmation method: {message}", ex.Message);
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant)
                    { ErrorDescription = "invalid subject confirmation method" };
                return;
            }
            catch (Exception ex) when (ex is SecurityTokenSignatureKeyNotFoundException || ex is SecurityTokenInvalidSignatureException)
            {
                _logger.LogWarning("The SAML assertion has an invalid signature: {message}", ex.Message);
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant)
                    { ErrorDescription = "Signature validation failed" };
                return;
            }
            catch (SecurityTokenInvalidAudienceException ex)
            {
                _logger.LogWarning("Audience validation failed. Invalid audience: {audience}.", ex.InvalidAudience);
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant)
                    { ErrorDescription = "Audience validation failed" };
                return;
            }
            catch (SecurityTokenExpiredException ex)
            {
                _logger.LogWarning("The SAML assertion is expired. Expiration Time: {time}", ex.Expires);
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant)
                    { ErrorDescription = "Assertion is expired" };
                return;
            }

            string subject = principal.Identity?.Name;

            if (subject == null)
            {
                _logger.LogWarning("Subject claim is missing in SAML assertion.");

                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant)
                { ErrorDescription = "Subject is missing" };
            }
            else
            {
                context.Result = new GrantValidationResult(subject, GrantType);
            }
        }
    }
}
