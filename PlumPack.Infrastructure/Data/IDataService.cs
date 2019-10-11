using System.Data;

namespace PlumPack.Infrastructure.Data
{
    public interface IDataService
    {
        IDbConnection OpenDbConnection();
    }
}