using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer4.Contrib.Saml2BearerGrant.Tests
{
    public static class Default
    {
        public static string Issuer => "http://idsrv";

        public static string RelyingParty => "http://relyingparty";

        public static string Subject => "userid";

        public static DateTime NotBefore => DateTime.Parse("2017-03-17T18:33:37.080Z");

        public static DateTime Expires => DateTime.Parse("2021-03-17T18:33:37.080Z");

        public static DateTime ExpiresInPast => DateTime.Parse("2017-03-18T18:33:37.080Z");

        public static List<Claim> SamlClaims => new List<Claim>
        {
            new Claim(ClaimTypes.Country, "USA", ClaimValueTypes.String, Issuer, Issuer),
            new Claim(ClaimTypes.NameIdentifier, "Bob", ClaimValueTypes.String, Issuer, Issuer),
            new Claim(ClaimTypes.Email, "Bob@contoso.com", ClaimValueTypes.String, Issuer, Issuer),
            new Claim(ClaimTypes.GivenName, "Bob", ClaimValueTypes.String, Issuer, Issuer),
            new Claim(ClaimTypes.HomePhone, "555.1212", ClaimValueTypes.String, Issuer, Issuer),
            new Claim(ClaimTypes.Role, "Developer", ClaimValueTypes.String, Issuer, Issuer),
            new Claim(ClaimTypes.Role, "Sales", ClaimValueTypes.String, Issuer, Issuer),
            new Claim(ClaimTypes.StreetAddress, "123AnyWhereStreet/r/nSomeTown/r/nUSA", ClaimValueTypes.String, Issuer, Issuer),
            new Claim(ClaimsIdentity.DefaultNameClaimType, "Jean-Sebastien", ClaimValueTypes.String, Issuer, Issuer),
        };
    }
}