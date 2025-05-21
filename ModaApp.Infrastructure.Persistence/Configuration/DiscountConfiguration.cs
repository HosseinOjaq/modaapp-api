using ModaApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ModaApp.Infrastructure.Persistence.Configuration;

internal class DiscountConfiguration : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.Property(x => x.Amount)
          .HasPrecision(5, 2)
          .IsRequired();

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Discount_Amount", "Amount >= 0 AND Amount <= 100");
        });

        builder.Property(x => x.Percent).IsRequired();
    }
}