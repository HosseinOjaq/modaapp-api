using ModaApp.Domain.Enums;

namespace ModaApp.Domain.Entities;
public class User
{
    public int Id { get; private set; }
    public string UserName { get; private set; } = default!;
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? Email { get; private set; }
    public bool EmailConfirmed { get; private set; }
    public string? PhoneNumber { get; private set; }
    public bool PhoneNumberConfirmed { get; private set; }
    public bool LoginPermission { get; private set; }
    public string PasswordHash { get; private set; } = null!;
    public GenderType? Gender { get; private set; }
    public bool IsActive { get; private set; } = true;
    public DateTime? LastLoginDateOnUtc { get; private set; }
    public int AccessFailedCount { get; private set; }
    public string SecurityStamp { get; private set; } = null!;
    public string? EconomicCode { get; private set; }


    public ICollection<Order> Orders { get; set; } = default!;
    public ICollection<Product> Products { get; set; } = default!;
    public ICollection<ProductLike> productLikes { get; set; } = default!;
    public ICollection<UserRole> UserRoles { get; private set; } = default!;
    public ICollection<ShoppingItem> ShippingItems { get; set; } = default!;
    public ICollection<ProductRating> ProductRatings { get; set; } = default!;
    public ICollection<UserAddress> UserAddresses { get; private set; } = default!;
    public ICollection<RefreshToken> RefreshTokens { get; private set; } = default!;

    public static User Create(string email, GenderType gender, string username,
                          string firstName, string lastName, string phoneNumber,
                          string passwordHash, string securityStamp)
    => new()
    {
        Email = email,
        Gender = gender,
        UserName = username,
        LastName = lastName,
        FirstName = firstName,
        PhoneNumber = phoneNumber,
        PasswordHash = passwordHash,
        SecurityStamp = securityStamp
    };

    public User SetUserRoles(List<UserRole> userRoles)
    {
        UserRoles = userRoles;
        return this;
    }
}