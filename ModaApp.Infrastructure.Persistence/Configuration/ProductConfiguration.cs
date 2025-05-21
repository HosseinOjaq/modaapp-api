using ModaApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ModaApp.Infrastructure.Persistence.Configuration;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder
            .Property(p => p.ShippingTime)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(p => p.Brand)
            .WithMany(c => c.products)
            .HasForeignKey(p => p.BrandId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Discount)
            .WithMany(c => c.products)
            .HasForeignKey(p => p.DiscountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(p => p.Title)
            .IsUnique();
    }
}