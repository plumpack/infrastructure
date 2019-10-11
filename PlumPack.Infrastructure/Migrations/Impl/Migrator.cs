using System;
using System.Linq;
using System.Threading.Tasks;
using PlumPack.Infrastructure.Data;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace PlumPack.Infrastructure.Migrations.Impl
{
    [Service(typeof(IMigrator))]
    public class Migrator : IMigrator
    {
        private readonly IDataService _dataService;
        private readonly IMigrationsBuilder _migrationsBuilder;

        public Migrator(IDataService dataService, IMigrationsBuilder migrationsBuilder)
        {
            _dataService = dataService;
            _migrationsBuilder = migrationsBuilder;
        }
        
        public void Migrate()
        {
            using (var db = _dataService.OpenDbConnection())
            {
                db.CreateTable<Migration>();

                var installedMigrations = db.Select<Migration>();

                using (var transaction = db.OpenTransaction())
                {
                    _migrationsBuilder.BuildMigrations(async migrations =>
                    {
                        if (migrations.Select(x => x.Version).Distinct().Count() != migrations.Count)
                        {
                            throw new Exception("Duplicate versions detected.");
                        }
                        
                        foreach (var migration in migrations)
                        {
                            if (installedMigrations.Any(x => x.Version == migration.Version))
                            {
                                // Already done!
                                continue;
                            }

                            migration.Run(db);

                            // Now that we ran the migration, let's add a record of it.
                            await db.InsertAsync(new Migration {Version = migration.Version, AppliedOn = DateTimeOffset.UtcNow});
                        }
                    });
                    
                    transaction.Commit();
                }
            }
        }

        public int? GetCurrentVersion()
        {
            using (var conScope = new ConScope(_dataService))
            {
                var db = conScope.Connection;
                
                db.CreateTable<Migration>();
                var migrations = db.Select<Migration>().ToList();
                if (migrations.Count == 0)
                {
                    return null;
                }
                return migrations.OrderByDescending(x => x.Version).First().Version;
            }
        }

        public class Migration
        {
            [AutoIncrement]
            public int Id { get; set; }
            
            public int Version { get; set; }
            
            public DateTimeOffset AppliedOn { get; set; }
        }
    }
}