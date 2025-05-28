using ModaApp.Domain.Enums;

namespace ModaApp.Application.Features.Permissions.Queries.HasPermission;

public record HasPermissionRequest
(
    int UserId,
    PermissionType PermissionType
);
