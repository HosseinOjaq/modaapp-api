using ModaApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModaApp.Infrastructure.Persistence.Extensions;

namespace ModaApp.Infrastructure.Persistence.Configuration;

internal class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.Property(t => t.Title)
               .IsRequired()
               .HasNVarcharMaxLength(400);

        builder.HasMany(t => t.productTags)
               .WithOne(pt => pt.Tags)
               .HasForeignKey(pt => pt.TagId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(t => t.Title).IsUnique();
    }
}