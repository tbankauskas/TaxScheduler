using IntegrationTests.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Taxes.Data.Ef;

namespace IntegrationTests.Infrastructure
{
    public class BaseTest
    {
        protected BaseTest()
        {
            var options = new DbContextOptionsBuilder<TaxesDbContext>()
                .UseSqlServer(DatabaseUtility.ConnectionString)
                .Options;
            var dbContext = new TaxesDbContext(options);

            SeedData.EnsureSeedData(dbContext);
        }
    }
}
