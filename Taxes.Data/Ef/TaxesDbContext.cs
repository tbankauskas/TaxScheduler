using Microsoft.EntityFrameworkCore;
using Taxes.Data.Ef.Configurations;
using Taxes.Data.Entities;

namespace Taxes.Data.Ef
{
    public class TaxesDbContext : DbContext
    {
        public TaxesDbContext(DbContextOptions<TaxesDbContext> options)
            : base(options)
        {
        }

        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<TaxType> TaxTypes { get; set; }
        public DbSet<TaxScheduler> TaxSchedulers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName(entityType.DisplayName());
            }

            modelBuilder.ApplyConfiguration(new MunicipalityConfiguration());
            modelBuilder.ApplyConfiguration(new TaxTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TaxSchedulerConfiguration());
        }
    }
}
