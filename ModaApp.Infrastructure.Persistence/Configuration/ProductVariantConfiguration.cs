using ModaApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ModaApp.Infrastructure.Persistence.Configuration;

internal class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.Property(p => p.Price)
       .HasPrecision(18, 2);

        builder.HasOne(p => p.ProductSize)
            .WithMany(c => c.ProductVariants)
            .HasForeignKey(p => p.ProductSizeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.ProductColor)
            .WithMany(c => c.ProductVariants)
            .HasForeignKey(p => p.ProductColorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Product)
            .WithMany(c => c.ProductVariants)
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}