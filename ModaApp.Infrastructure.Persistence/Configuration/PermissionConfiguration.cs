using ModaApp.Domain.Enums;
using ModaApp.Domain.Entities;
using ModaApp.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using ModaApp.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ModaApp.Infrastructure.Persistence.Configuration;

internal class PermissionConfig : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(p => p.Title)
            .HasNVarcharMaxLength(40)
            .IsRequired();

        builder.Property(p => p.NameSpace)
            .HasVarcharMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Url)
            .HasVarcharMaxLength(100)
            .IsRequired();

        builder.HasOne(x => x.Parent)
            .WithMany(x => x.Children)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.RolePermissions)
            .WithOne(x => x.Permission)
            .HasForeignKey(x => x.PermissionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(Permission.Create(PermissionType.Admin, "", PermissionType.Admin.ToDisplay(), "", 0, PermissionLevelType.Product));
    }
}