using ModaApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ModaApp.Infrastructure.Persistence.Configuration;

internal class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.Property(p => p.Authority)
               .HasMaxLength(100);

        builder.Property(p => p.Date)
               .IsRequired();

        builder.Property(p => p.StatusCode)
               .IsRequired();

        builder.Property(p => p.Status)
               .IsRequired();

        builder.Property(p => p.RefrencID)
               .IsRequired();

        builder.Property(p => p.Price)
               .HasPrecision(18, 2);

        builder.HasOne(p => p.Order)
               .WithMany(o => o.Payments)
               .HasForeignKey(p => p.OrderId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}