using ModaApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ModaApp.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ModaApp.Infrastructure.Persistence.Configuration;

internal class ProductColorConfiguration : IEntityTypeConfiguration<ProductColor>
{
    public void Configure(EntityTypeBuilder<ProductColor> builder)
    {
        builder.Property(x => x.Title)
            .HasNVarcharMaxLength(100)
               .IsRequired();

        builder.HasMany(x => x.ProductVariants)
               .WithOne(x => x.ProductColor)
               .HasForeignKey(x => x.ProductColorId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}