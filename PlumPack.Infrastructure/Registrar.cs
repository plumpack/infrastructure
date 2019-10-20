using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PlumPack.Infrastructure.Email;
using PlumPack.Infrastructure.Migrations;
using ServiceStack.Data;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.PostgreSQL;

namespace PlumPack.Infrastructure
{
    public static class Registrar
    {
        public static void Register(IServiceCollection services, IConfiguration configuration, MigrationOptions migrationOptions, string addtionalConfigDirectory)
        {
            // Scan for all the service attributes in this assembly.
            ServiceContext.AddServicesFromAssembly(typeof(Registrar).Assembly, services);
            
            // Register the connection factory for database access.
            var connectionString = configuration.GetConnectionString("Postgres");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("No connection string for postgres");
            }
            var factory = new OrmLiteConnectionFactory(
                connectionString,  
                PostgreSqlDialectProvider.Instance);
            services.AddSingleton<IDbConnectionFactory>(factory);

            // Add our migrations options, used to find the assembly which contains the migrations.
            services.AddSingleton(migrationOptions);

            services.Configure<PlumPackOptions>(configuration.GetSection("PlumPack"));
            services.Configure<EmailOptions>(configuration.GetSection("Email"));

            var emailYml = Path.Combine(addtionalConfigDirectory, "email.yml");
            if (File.Exists(emailYml))
            {
                services.Configure<EmailOptions>(new ConfigurationBuilder().AddYamlFile(emailYml).Build());
            }
            
            var plumPackYml = Path.Combine(addtionalConfigDirectory, "plumpack.yml");
            if (File.Exists(plumPackYml))
            {
                services.Configure<PlumPackOptions>(new ConfigurationBuilder().AddYamlFile(plumPackYml).Build());
            }
        }
    }
}