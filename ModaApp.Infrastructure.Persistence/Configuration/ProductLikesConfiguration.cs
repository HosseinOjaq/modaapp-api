using ModaApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ModaApp.Infrastructure.Persistence.Configuration;

internal class ProductLikesConfiguration : IEntityTypeConfiguration<ProductLike>
{
    public void Configure(EntityTypeBuilder<ProductLike> builder)
    {
        builder.HasIndex(x => new { x.ProductId, x.UserId }).IsUnique();
        builder.HasOne<Product>()
               .WithMany(p => p.productLikes)
               .HasForeignKey(x => x.ProductId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pl => pl.User)
               .WithMany(u => u.productLikes)
               .HasForeignKey(pl => pl.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}