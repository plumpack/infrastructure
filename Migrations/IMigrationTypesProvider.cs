using System;
using System.Collections.Generic;

namespace PlumPack.Infrastructure.Migrations
{
    public interface IMigrationTypesProvider
    {
        IList<Type> GetMigrationTypes();
    }
}