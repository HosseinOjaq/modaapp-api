using ModaApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ModaApp.Infrastructure.Persistence.Configuration;

internal class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.OrderSum)
               .HasPrecision(18, 2)
               .IsRequired();

        builder.Property(o => o.IsFinaly)
               .IsRequired();

        builder.Property(o => o.CreateDate)
               .IsRequired();

        builder.Property(o => o.Code)
               .HasDefaultValueSql("NEXT VALUE FOR dbo.OrderCodeSequence");

        builder.HasOne(o => o.User)
               .WithMany(u => u.Orders)
               .HasForeignKey(o => o.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(o => o.Payments)
               .WithOne(p => p.Order)
               .HasForeignKey(p => p.OrderId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(o => o.OrderDetails)
               .WithOne(od => od.Order)
               .HasForeignKey(od => od.OrderId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}