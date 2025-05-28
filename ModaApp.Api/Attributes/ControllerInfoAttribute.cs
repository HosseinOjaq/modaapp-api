using ModaApp.Domain.Enums;

namespace ModaApp.Api.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ControllerInfoAttribute : Attribute
{
    public PermissionType PermissionType { get; }
    public PermissionType GroupType { get; }

    public ControllerInfoAttribute(PermissionType groupType)
    {
        GroupType = groupType;
    }

    public ControllerInfoAttribute(PermissionType permissionType, PermissionType groupType)
    {
        PermissionType = permissionType;
        GroupType = groupType;
    }
}