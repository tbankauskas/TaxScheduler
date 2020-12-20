using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Taxes.Data.Entities;

namespace Taxes.Data.Ef
{
    public static class SeedData
    {
        public static async Task EnsureSeedData(IServiceProvider provider)
        {
            var dbContext = provider.GetRequiredService<TaxesDbContext>();
            await dbContext.Database.MigrateAsync();

            if (!await dbContext.TaxTypes.AnyAsync())
            {
                await dbContext.TaxTypes.AddRangeAsync(GetTaxTypes());
                await dbContext.SaveChangesAsync();
            }
        }

        private static List<TaxType> GetTaxTypes()
        {
            return new List<TaxType>
            {
                new TaxType {TaxTypeId = 1, TypeName = "Daily", Priority = 1},
                new TaxType {TaxTypeId = 2, TypeName = "Weekly", Priority = 2},
                new TaxType {TaxTypeId = 3, TypeName = "Monthly", Priority = 3},
                new TaxType {TaxTypeId = 4, TypeName = "Yearly", Priority = 4},
            };
        }
    }
}
