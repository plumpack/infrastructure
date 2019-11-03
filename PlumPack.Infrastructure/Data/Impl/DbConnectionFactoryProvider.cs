using System;
using Microsoft.Extensions.Options;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.PostgreSQL;
using SharpDataAccess.Data;

namespace PlumPack.Infrastructure.Data.Impl
{
    [Service(typeof(IDbConnectionFactoryProvider))]
    public class DbConnectionFactoryProvider : IDbConnectionFactoryProvider
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        
        public DbConnectionFactoryProvider(IOptions<ConnectionStrings> connectionStringsOptions)
        {
            var connectionString = connectionStringsOptions.Value?["Postgres"];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("No connecton string provided.");
            }

            ServiceStack.Licensing.RegisterLicense("7315-e1JlZjo3MzE1LE5hbWU6TWVkWENoYW5nZSxUeXBlOkluZGllLE1ldGE6MCxI YXNoOk13TW54QkdqWENJNlZpVkF3dzdyYXBIN25sOW1OUjQyR0V4QkxiQ1BJN1Fvc mt6Sm9VMW9uVTBNbytlckRrNEVwMFFOTjZlaURYY0MzUXlVZVpVOEJIdDlPbDVBOF VUYTdzSXlha3lFUHAyWnU5bGlJU08xMTJZRVZSU1ZGRkdhUGJCRnR4NmN6bFdXWnF ubG5WMWlUWnJ6VkhNR0U0b29KOHBTazVOWVlCMD0sRXhwaXJ5OjIwMjAtMDYtMDR9");
            _dbConnectionFactory = new OrmLiteConnectionFactory(
                connectionString,  
                PostgreSqlDialectProvider.Instance);
        }

        public IDbConnectionFactory BuildConnectionFactory()
        {
            return _dbConnectionFactory;
        }
    }
}