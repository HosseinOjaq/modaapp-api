using ModaApp.Application.Features.Permissions.Queries.HasPermission;
using ModaApp.Domain.Enums;

namespace ModaApp.Application.Services.Interfaces;

public interface IPermissionService
{
    Task<HasPermissionResponse> HasUserPermissionAsync(int userId, PermissionType permissionType);
}

