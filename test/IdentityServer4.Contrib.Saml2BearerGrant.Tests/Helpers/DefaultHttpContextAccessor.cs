using Microsoft.AspNetCore.Http;

namespace IdentityServer4.Contrib.Saml2BearerGrant.Tests.Helpers
{
    public class DefaultHttpContextAccessor : IHttpContextAccessor
    {
        public HttpContext HttpContext { get; set; }

        private readonly DefaultServiceProvider _serviceProvider;

        public DefaultHttpContextAccessor()
        {
            HttpContext = new DefaultHttpContext();
            _serviceProvider = new DefaultServiceProvider();
            HttpContext.RequestServices = _serviceProvider;
        }

        public void SetService<T>(T instance) where T : class
        {
            _serviceProvider.Services[typeof(T)] = instance;
        }
    }
}
