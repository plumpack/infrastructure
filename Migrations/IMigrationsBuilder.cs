using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlumPack.Infrastructure.Migrations
{
    public interface IMigrationsBuilder
    {
        void BuildMigrations(Action<IList<IMigration>> action);
    }
}