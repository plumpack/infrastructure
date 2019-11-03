using System;
using SharpDataAccess.Migrations;

namespace PlumPack.Infrastructure.Migrations.Impl
{
    [Service(typeof(IMigrationBuilder))]
    public class MigrationBuilder : IMigrationBuilder
    {
        public IMigration Create(Type migrationType)
        {
            return Activator.CreateInstance(migrationType) as IMigration;
        }
    }
}