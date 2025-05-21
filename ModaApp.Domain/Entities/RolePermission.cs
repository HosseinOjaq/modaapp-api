using ModaApp.Domain.Enums;


namespace ModaApp.Domain.Entities;

public class RolePermission
{
    public int Id { get; private set; }
    public int RoleId { get; private set; }
    public PermissionType PermissionId { get; private set; }


    public Permission? Permission { get; private set; }
    public Role? Role { get; private set; }

    public static RolePermission Create(int roleId, PermissionType PermissionId)
    => new()
    {
        RoleId = roleId,
        PermissionId = PermissionId
    };
}