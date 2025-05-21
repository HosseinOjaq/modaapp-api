using ModaApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ModaApp.Infrastructure.Persistence.Configuration;

internal class ProductTagsConfiguration : IEntityTypeConfiguration<ProductTags>
{
    public void Configure(EntityTypeBuilder<ProductTags> builder)
    {
        builder.HasOne(p => p.Tags)
            .WithMany(c => c.productTags)
            .HasForeignKey(p => p.TagId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Product)
            .WithMany(c => c.ProductTags)
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}