using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taxes.Data.Entities;

namespace Taxes.Data.Ef.Configurations
{
    public class TaxSchedulerConfiguration : IEntityTypeConfiguration<TaxScheduler>
    {
        public void Configure(EntityTypeBuilder<TaxScheduler> builder)
        {
            builder
                .Property(x => x.TaxValue)
                .IsRequired()
                .HasColumnType("decimal")
                .HasPrecision(4, 2);
        }
    }
}
