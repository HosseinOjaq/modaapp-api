namespace ModaApp.Domain.Entities;

public class UserRole
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public int RoleId { get; private set; }


    public Role Role { get; private set; } = default!;
    public User User { get; private set; } = default!;

    public static UserRole Create(int roleId)
    => new()
    {
        RoleId = roleId
    };
}