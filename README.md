# SAML 2.0/1.1 bearer grant types for IdentityServer4

Extension grant types for IdentityServer4 implementing a subset of [RFC 7522](https://tools.ietf.org/html/rfc7522).
* Adds support for SAML 2.0 and SAML 1.1 assertions as grant for token requests.
* New grant types ```urn:ietf:params:oauth:grant-type:saml2-bearer``` and ```urn:ietf:params:oauth:grant-type:saml-bearer``` (WS-Federation uses SAML 1.1 assertions).

You can add the extension grant types to IdentityServer using the extension builder methods ```AddSamlBearerGrant()``` and ```AddSaml2BearerGrant()```.

This extension was built for legacy applications using SAML or WS-Federation authentication.
Today still a lot of companies are using these authentication protocols in their legacy business applications.
For example SharePoint is using the WS-Federation protocol for federated authentication.
These applications have no chance to get an OAuth2 access token with an user as subject.

With this extension grant they can request an access token from the Token Endpoint by a
valid SAML assertion and client credentials. The access token will contain the user id
which was the subject of the SAML assertion.

## Deviations from RFC 7522

* We do not support self-issued (by the OAuth clients) assertions nor 3rd-Party security token services.
  Only assertions issued by IdentityServer as SAML-IdP will work as grant.
* This library doesn't supports SAML Assertions for Client Authentication.
* Client id and secret are required for client authentication for each token request.
* We do not validate the <Audience> element because we except the Relying Party URI as audience for SAML authentication.
  According to RFC 7522 the <Audience> element must contain the Token Endpoint URL of the Identity Provider.

## .NET and IdentityServer Support

This library is targeting .NET Core 3.1 and IdentityServer4 release 3.0.0+.

## How to use

### Server

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddIdentityServer()
        .AddSaml2BearerGrant()
        // allow SAML 1.1 assertions to support WS-Federation clients.
        .AddSamlBearerGrant()
}
```

### Client

Use a SAML assertion and client credentials to get an access token that is subjected to the user:

```
POST /connect/token

grant_type=urn:ietf:params:oauth:grant-type:saml2-bearer&
scope=openid api1&
assertion=<base64-url-encoded-assertion>&
client_id=<my-oidc-client-id>
client_secret=secret
```
