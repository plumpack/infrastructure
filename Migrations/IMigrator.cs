using System.Threading.Tasks;

namespace PlumPack.Infrastructure.Migrations
{
    public interface IMigrator
    {
        void Migrate();

        int? GetCurrentVersion();
    }
}