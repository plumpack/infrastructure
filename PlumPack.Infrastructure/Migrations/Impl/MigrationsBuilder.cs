using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlumPack.Infrastructure.Data;

namespace PlumPack.Infrastructure.Migrations.Impl
{
    [Service(typeof(IMigrationsBuilder))]
    public class MigrationsBuilder : IMigrationsBuilder
    {
        private readonly IMigrationTypesProvider _migrationTypesProvider;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IDataService _dataService;

        public MigrationsBuilder(IMigrationTypesProvider migrationTypesProvider,
            ILoggerFactory loggerFactory,
            IDataService dataService)
        {
            _migrationTypesProvider = migrationTypesProvider;
            _loggerFactory = loggerFactory;
            _dataService = dataService;
        }
        
        public void BuildMigrations(Action<IList<IMigration>> action)
        {
            var services = new ServiceCollection();
            
            foreach (var migrationType in _migrationTypesProvider.GetMigrationTypes())
            {
                services.AddScoped(typeof(IMigration), migrationType);
            }

            services.AddSingleton(_loggerFactory);
            services.AddSingleton(_dataService);
            
            var serviceProvider = services.BuildServiceProvider();

            try
            {
                var migrations = serviceProvider.GetRequiredService<IEnumerable<IMigration>>()
                    .OrderBy(x => x.Version).ToList();

                if (migrations.Select(x => x.Version).Distinct().Count() != migrations.Count)
                {
                    throw new Exception("Duplicate versions detected.");
                }

                action(migrations);
            }
            finally
            {
                serviceProvider.Dispose();
            }
        }
    }
}