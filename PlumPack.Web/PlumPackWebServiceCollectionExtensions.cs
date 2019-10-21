using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlumPack.Web.Mvc;
using PlumPack.Web.Validation;

namespace PlumPack.Web
{
    public static class ServiceCollectionExtensions
    {
        public static ServiceExtensions PlumPack(this IServiceCollection services, IWebHostEnvironment env)
        {
            return new ServiceExtensions {Services = services, Env = env};
        }
        
        public static ServiceExtensions AddControllersWithViews(this ServiceExtensions services)
        {
            var builder = services.Services.AddControllersWithViews(options =>
                {
                    // add the "feature" convention
                    options.Conventions.Add(new FeatureConvention());
                    // Auto add [Area("areaName"] to controllers.
                    options.Conventions.Add(new AutoAreaConvention());
                })
                .AddRazorOptions(options =>
                {
                    // using the "feature" convention, expand the paths
                    options.ViewLocationExpanders.Add(new FeatureViewLocationExpander());
                })
                .AddFluentValidation();

            if (services.Env.IsDevelopment())
            {
                builder.AddRazorRuntimeCompilation();
            }

            return services;
        }

        public static void AddValidators(this ServiceExtensions services, Assembly assembly)
        {
            // Add all of our validators
            foreach (var validator in ValidatorDiscovery.DiscoverValidators(assembly))
            {
                services.Services.AddTransient(validator.Interface, validator.Implementation);
            }
        }

        public class ServiceExtensions
        {
            public IServiceCollection Services { get; set; }
            
            public IWebHostEnvironment Env { get; set; }
        }
    }
}