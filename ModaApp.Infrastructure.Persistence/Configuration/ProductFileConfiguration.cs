using ModaApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ModaApp.Infrastructure.Persistence.Configuration;

internal class ProductFileConfiguration : IEntityTypeConfiguration<ProductFile>
{
    public void Configure(EntityTypeBuilder<ProductFile> builder)
    {
        builder.Property(p => p.FileName).IsRequired().HasMaxLength(200);
        builder.HasOne(p => p.Product)
               .WithMany(c => c.ProductFiles)
               .HasForeignKey(p => p.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}