using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taxes.Data.Ef;

namespace IntegrationTests.Infrastructure.Utilities
{
    public static class DatabaseUtility
    {
        public const string ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=TaxesDb.Testing;Trusted_Connection=True;";

        public static async Task CleanUp()
        {
            await using var context = CreateDbContext();
            await context.Database.ExecuteSqlRawAsync(
                @"
                    TRUNCATE TABLE [TaxScheduler]
                    DELETE FROM [Municipality];
                 "
            );
        }

        public static TaxesDbContext CreateDbContext()
        {
            return new TaxesDbContext(GetDbContextOptions<TaxesDbContext>());
        }

        private static DbContextOptions<T> GetDbContextOptions<T>() where T : DbContext
        {
            return new DbContextOptionsBuilder<T>()
                .UseSqlServer(ConnectionString)
                .Options;
        }
    }
}
