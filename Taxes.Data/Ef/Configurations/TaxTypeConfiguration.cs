using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taxes.Data.Entities;

namespace Taxes.Data.Ef.Configurations
{
    public class TaxTypeConfiguration : IEntityTypeConfiguration<TaxType>
    {
        public void Configure(EntityTypeBuilder<TaxType> builder)
        {
            builder.Property(x => x.TaxTypeId).ValueGeneratedNever();
            builder
                .Property(x => x.TypeName)
                .IsRequired();

            builder
                .HasIndex(x => x.TypeName)
                .IsUnique();
        }
    }
}
