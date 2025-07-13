using System.Reflection;
using ModaApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ModaApp.Infrastructure.Persistence.Interceptors;

namespace ModaApp.Infrastructure.Persistence;

public sealed class ModaAppDbContext
    (DbContextOptions<ModaAppDbContext> options)
    : DbContext(options)
{
    public DbSet<Category> Category { get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<Permission> Permission { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<Discount> Discount { get; set; }
    public DbSet<ProductFile> ProductFile { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<RolePermission> RolePermission { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<UserAddress> UserAddress { get; set; }
    public DbSet<UserRole> UserRole { get; set; }
    public DbSet<Tag> Tag { get; set; }
    public DbSet<ProductTags> ProductTag { get; set; }
    public DbSet<ProductSize> ProductSize { get; set; }
    public DbSet<ProductVariant> ProductVariant { get; set; }
    public DbSet<ShoppingItem> ShoppingItem { get; set; }
    public DbSet<ProductColor> ProductColor { get; set; }
    public DbSet<Payment> Payment { get; set; }
    public DbSet<OrderDetail> OrderDetail { get; set; }
    public DbSet<Category> Categorie { get; set; }
    public DbSet<Brand> Brand { get; set; }
    public DbSet<PageVisit> PageVisits { get; set; }
    public DbSet<ProductLike> ProductLike { get; set; }
    public DbSet<ProductRating> ProductRating { get; set; }
    public DbSet<RefreshToken> RefreshToken { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new CleanStringPropertyInterceptor());
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.HasSequence<int>("OrderCodeSequence", schema: "dbo")
                    .StartsAt(1000)
                    .IncrementsBy(1);

        base.OnModelCreating(modelBuilder);
    }
}