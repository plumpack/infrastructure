using System;
using System.Reflection;

namespace PlumPack.Infrastructure.Migrations
{
    public class MigrationOptions
    {
        public MigrationOptions(Assembly migrationsAssembly)
        {
            if (migrationsAssembly == null)
            {
                throw new Exception("You must provide a migrations assembly");
            }
            
            MigrationsAssembly = migrationsAssembly;
        }
        
        public Assembly MigrationsAssembly { get; }
    }
}