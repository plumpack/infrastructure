using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PlumPack.Infrastructure.Data;
using PlumPack.Infrastructure.Email;
using PlumPack.Infrastructure.Migrations;
using PlumPack.Infrastructure.Migrations.Impl;
using ServiceStack.Data;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.PostgreSQL;
using SharpDataAccess.Data;
using SharpDataAccess.Migrations;

namespace PlumPack.Infrastructure
{
    public static class Registrar
    {
        public static void Register(IServiceCollection services, IConfiguration configuration, MigrationOptions migrationOptions, string additionalConfigDirectory)
        {
            // Scan for all the service attributes in this assembly.
            ServiceContext.AddServicesFromAssembly(typeof(Registrar).Assembly, services);

            services.Configure<ConnectionStrings>(configuration?.GetSection("ConnectionStrings"));
            
            // Register the types for SharpDataAccess
            services.AddSingleton<IDataService, SharpDataAccess.Data.Impl.DataService>();
            services.AddSingleton<IMigrator, SharpDataAccess.Migrations.Impl.Migrator>();
            services.AddSingleton<IMigrationsBuilder, SharpDataAccess.Migrations.Impl.MigrationsBuilder>();
            services.AddSingleton<IMigrationBuilder, MigrationBuilder>();
            services.AddSingleton<IMigrationTypesProvider, SharpDataAccess.Migrations.Impl.MigrationTypesProvider>();
            services.AddSingleton(migrationOptions);

            services.Configure<PlumPackOptions>(configuration.GetSection("PlumPack"));
            services.Configure<EmailOptions>(configuration.GetSection("Email"));

            var emailYml = Path.Combine(additionalConfigDirectory, "email.yml");
            if (File.Exists(emailYml))
            {
                services.Configure<EmailOptions>(new ConfigurationBuilder().AddYamlFile(emailYml).Build());
            }
            
            var plumPackYml = Path.Combine(additionalConfigDirectory, "plumpack.yml");
            if (File.Exists(plumPackYml))
            {
                services.Configure<PlumPackOptions>(new ConfigurationBuilder().AddYamlFile(plumPackYml).Build());
            }
        }
    }
}