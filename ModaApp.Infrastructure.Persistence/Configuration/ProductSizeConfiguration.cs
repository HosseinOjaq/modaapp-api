using ModaApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ModaApp.Infrastructure.Persistence.Configuration;

internal class ProductSizeConfiguration : IEntityTypeConfiguration<ProductSize>
{
    public void Configure(EntityTypeBuilder<ProductSize> builder)
    {
        builder.Property(p => p.title)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasMany(p => p.ProductVariants)
               .WithOne(pv => pv.ProductSize)
               .HasForeignKey(pv => pv.ProductSizeId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}