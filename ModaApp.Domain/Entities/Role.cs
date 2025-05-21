namespace ModaApp.Domain.Entities;

public class Role
{
    public int Id { get; private set; }
    public string Title { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;


    public ICollection<RolePermission> RolePermissions { get; private set; } = default!;
    public ICollection<UserRole> UserRoles { get; private set; } = default!;


    public static Role Create(string title)
    => new()
    {
        Title = title
    };
}