using System.Data;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace PlumPack.Infrastructure.Data.Impl
{
    [Service(typeof(IDataService))]
    public class DataService : IDataService
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public DataService(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
            ServiceStack.Licensing.RegisterLicense("7315-e1JlZjo3MzE1LE5hbWU6TWVkWENoYW5nZSxUeXBlOkluZGllLE1ldGE6MCxI YXNoOk13TW54QkdqWENJNlZpVkF3dzdyYXBIN25sOW1OUjQyR0V4QkxiQ1BJN1Fvc mt6Sm9VMW9uVTBNbytlckRrNEVwMFFOTjZlaURYY0MzUXlVZVpVOEJIdDlPbDVBOF VUYTdzSXlha3lFUHAyWnU5bGlJU08xMTJZRVZSU1ZGRkdhUGJCRnR4NmN6bFdXWnF ubG5WMWlUWnJ6VkhNR0U0b29KOHBTazVOWVlCMD0sRXhwaXJ5OjIwMjAtMDYtMDR9");
        }
        
        public IDbConnection OpenDbConnection()
        {
            return _dbConnectionFactory.OpenDbConnection();
        }
    }
}