using ModaApp.Domain.Enums;

namespace ModaApp.Domain.Entities;
public class Permission
{
    public PermissionType Id { get; private set; }
    public string Title { get; private set; } = null!;
    public string Url { get; private set; } = null!;
    public string? NameSpace { get; private set; }
    public PermissionType? ParentId { get; private set; }
    public PermissionLevelType LevelTypeId { get; private set; }
    public int Priority { get; private set; }
    public bool IsActive { get; private set; } = true;


    public Permission Parent { get; private set; } = default!;
    public ICollection<Permission> Children { get; private set; } = default!;
    public ICollection<RolePermission> RolePermissions { get; private set; } = default!;


    public static Permission Create(PermissionType id, string url, string title, string nameSpace,
                                int Priority, PermissionLevelType levelType, PermissionType? parentId = null)
    => new()
    {
        Id = id,
        Url = url,
        Title = title,
        ParentId = parentId,
        NameSpace = nameSpace,
        Priority = Priority,
        LevelTypeId = levelType
    };

    public void SetParents(ICollection<Permission> parents)
    {
        Children = parents;
    }
}