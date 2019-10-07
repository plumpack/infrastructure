using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlumPack.Infrastructure.Migrations;
using ServiceStack.Data;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.PostgreSQL;

namespace PlumPack.Infrastructure
{
    public static class Registrar
    {
        public static void Register(IServiceCollection services, IConfiguration configuration, MigrationOptions migrationOptions)
        {
            LogManager.LogFactory = new ConsoleLogFactory();
            
            ServiceContext.AddServicesFromAssembly(typeof(Registrar).Assembly, services);
            
            var connectionString = configuration.GetConnectionString("Postgres");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("No connection string for postgres");
            }

            services.AddSingleton(migrationOptions);
            
            var factory = new OrmLiteConnectionFactory(
                connectionString,  
                PostgreSqlDialectProvider.Instance);
            services.AddSingleton<IDbConnectionFactory>(factory);
            
            ServiceContext.AddServicesFromAssembly(typeof(Registrar).Assembly, services);
        }
    }
}