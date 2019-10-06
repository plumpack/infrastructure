using System;
using Microsoft.Extensions.DependencyInjection;

namespace PlumPack.Infrastructure
{
    public static class ServiceRegistration
    {
        public delegate object ServiceBuilderDelegate(IServiceProvider provider, Type service, Type implementation);
        
        public static void Register(IServiceCollection services, ServiceAttribute attribute, Type declaredType, ServiceBuilderDelegate instantiate = null)
        {
            if (instantiate == null)
            {
                instantiate = (provider, service, implementation) =>
                    ActivatorUtilities.CreateInstance(provider, implementation);
            }
            
            if (declaredType == null)
            {
                // Not supported (yet)
                throw new NotImplementedException();
            }

            var serviceType = attribute.ServiceType;
            if (serviceType == null)
            {
                serviceType = declaredType;
            }

            if (declaredType.IsGenericType)
            {
                // We are registering services on a generic type implementation
                declaredType = declaredType.MakeGenericType(serviceType.GetGenericArguments());
            }
            
            switch (attribute.Lifetime)
            {
                case ServiceLifetime.Scoped:
                    services.AddScoped(serviceType, provider => instantiate(provider, serviceType, declaredType));
                    break;
                case ServiceLifetime.Singleton:
                    services.AddSingleton(serviceType, provider => instantiate(provider, serviceType, declaredType));
                    break;
                case ServiceLifetime.Transient:
                    services.AddTransient(serviceType, provider => instantiate(provider, serviceType, declaredType));
                    break;
            }
        }
    }
}