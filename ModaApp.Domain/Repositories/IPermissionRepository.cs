using ModaApp.Domain.Enums;

namespace ModaApp.Domain.Repositories;

public interface IPermissionRepository
{
    Task<bool> HasUserPermissionAsync(int userId, PermissionType permissionType);
}
