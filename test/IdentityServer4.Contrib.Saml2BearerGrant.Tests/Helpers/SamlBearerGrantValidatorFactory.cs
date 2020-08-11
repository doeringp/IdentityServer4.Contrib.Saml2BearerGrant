using IdentityServer4.Configuration;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Contrib.Saml2BearerGrant.Tests.Helpers
{
    public class SamlBearerGrantValidatorFactory
    {
        public DefaultHttpContextAccessor HttpContextAccessor { get; set; } = new DefaultHttpContextAccessor();

        public HttpContext HttpContext => HttpContextAccessor.HttpContext;

        public SigningCredentials SigningCredentials
        {
            get
            {
                var key = KeyingMaterial.X509SecurityKeySelfSigned2048_SHA256;
                return new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest);
            }
        }

        public SamlBearerGrantValidatorFactory()
        {
            HttpContext.SetIdentityServerOrigin(Default.Issuer);
            HttpContext.SetIdentityServerBasePath("/");

            var idsrvOptions = new IdentityServerOptions();
            HttpContextAccessor.SetService(idsrvOptions);
        }

        public T CreateValidator<T>() where T : BaseSamlBearerGrantValidator
        {
            var logger = new Mock<ILogger<T>>();
            var keys = new Mock<IKeyMaterialService>();

            keys.Setup(x => x.GetSigningCredentialsAsync())
                .Returns(Task.FromResult(SigningCredentials));

            keys.Setup(x => x.GetValidationKeysAsync())
                .Returns(Task.FromResult(new[] { SigningCredentials.Key }.AsEnumerable()));

            var systemClock = new SystemClock();

            return (T) Activator.CreateInstance(typeof(T),
                keys.Object, HttpContextAccessor, logger.Object, systemClock);
        }
    }
}
