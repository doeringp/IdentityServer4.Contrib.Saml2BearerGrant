using System;
using System.Collections.Generic;

namespace IdentityServer4.Contrib.Saml2BearerGrant.Tests.Helpers
{
    public class DefaultServiceProvider : IServiceProvider
    {
        public IDictionary<Type, object> Services { get; } = new Dictionary<Type, object>();

        public object GetService(Type serviceType)
        {
            if (Services.ContainsKey(serviceType))
            {
                return Services[serviceType];
            }
            throw new Exception($"The service of type {serviceType} is not available.");
        }
    }
}
