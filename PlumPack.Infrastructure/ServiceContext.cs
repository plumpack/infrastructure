using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace PlumPack.Infrastructure
{
    public class ServiceContext
    {
        public static void AddServicesFromAssembly(Assembly assembly, IServiceCollection services)
        {
            foreach (var type in assembly.GetTypes())
            {
                TryRegisterService(type, services);
            }
        }
        
        public static void TryRegisterService(Type type, IServiceCollection services)
        {
            if (!type.IsClass || type.IsAbstract)
            {
                return;
            }

            var serviceAttributes = type.GetCustomAttributes(typeof(ServiceAttribute), false)
                .OfType<ServiceAttribute>().ToList();

            foreach (var serviceAttribute in serviceAttributes)
            {
                ServiceRegistration.Register(services, serviceAttribute, type);
            }
        }
    }
}