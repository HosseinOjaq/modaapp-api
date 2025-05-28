using ModaApp.Domain.Enums;
using ModaApp.Domain.Repositories;
using ModaApp.Application.Services.Interfaces;
using ModaApp.Application.Features.Permissions.Queries.HasPermission;

namespace ModaApp.Application.Services.Implementations;

public class PermissionService(IPermissionRepository permissionRepository) : IPermissionService
{
    public async Task<HasPermissionResponse> HasUserPermissionAsync(int userId, PermissionType permissionType)
    {
        var hasPermission = await permissionRepository.HasUserPermissionAsync(userId , permissionType);

        return new HasPermissionResponse(hasPermission);
    }
}
