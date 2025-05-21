namespace ModaApp.Domain.Entities;

public class UserAddress
{
    public int UserId { get; set; }
    public required string Title { get; set; }
    public required string Address { get; set; }
    public required string Postalcode { get; set; }
    public bool IsDefault { get; set; }

    public User User { get; set; } = default!;
}