using System.Data;
using System.Threading.Tasks;

namespace PlumPack.Infrastructure.Migrations
{
    public interface IMigration
    {
        void Run(IDbConnection connection);
        
        int Version { get; }
    }
}