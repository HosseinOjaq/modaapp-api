using ModaApp.Domain.Enums;
using ModaApp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ModaApp.Infrastructure.Persistence.Repositories;

public class PermissionRepository
    (ModaAppDbContext context)
    : IPermissionRepository
{
    public async Task<bool> HasUserPermissionAsync(int userId, PermissionType permissionType)
    {
        return await (from UserRole in context.UserRole
                      join RolePermission in context.RolePermission
                      on UserRole.RoleId equals RolePermission.RoleId
                      where UserRole.UserId == userId
                      && RolePermission.PermissionId == permissionType
                      select RolePermission.PermissionId)
                   .AnyAsync();
    }
}