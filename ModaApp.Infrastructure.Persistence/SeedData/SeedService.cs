using ModaApp.Domain.Enums;
using ModaApp.Domain.Entities;
using ModaApp.Common.Utilities;
using ModaApp.Application.Models;
using Microsoft.EntityFrameworkCore;
using ModaApp.Application.Common.Contracts;

namespace ModaApp.Infrastructure.Persistence.SeedData;

public class SeedService(ModaAppDbContext context) : ISeedService
{
    public async Task SeedDataAsync(List<DynamicPermission> dynamicPermissions)
    {
        var adminRoleId = await SeedRolesAsync();
        await SeedUsersAsync(adminRoleId);

        var permissionIds = await SeedPermissionsAsync(dynamicPermissions);
        if (permissionIds.Count != 0)
            await SeedRolePermissionsAsync(permissionIds, adminRoleId);

    }
    private async Task<int> SeedRolesAsync()
    {
        var roles = new List<Role>
        {
            Role.Create("مدیر"),
            Role.Create("فروشنده")
        };
        var existsRoles = await context.Role.AsNoTracking().ToListAsync();
        roles = roles.Where(role => !existsRoles.Any(x => x.Title == role.Title)).ToList();

        if (roles.Count != 0)
        {
            context.Role.AddRange(roles);
            await context.SaveChangesAsync();
        }

        var adminRoleId =
            roles.Count != 0
            ? roles.First().Id
            : existsRoles.Single(x => x.Title == "مدیر" || x.Title.Contains("مدیر")).Id;

        return adminRoleId;
    }

    private async Task<List<int>> SeedPermissionsAsync(List<DynamicPermission> dynamicPermissions)
    {
        var priority = 1;
        foreach (var dynamicPermission in dynamicPermissions)
        {
            var existsTabPermission = await context
                             .Permission
                             .FirstOrDefaultAsync(x => x.Title == dynamicPermission.Name && x.ParentId == PermissionType.Admin);

            var tabPermission = existsTabPermission ??
                                Permission.Create(dynamicPermission.Controllers[0].GroupType, "", dynamicPermission.Name, "",
                                                  priority, PermissionLevelType.Tab, PermissionType.Admin);
            tabPermission.SetParents([]);

            var versionOfControllers = dynamicPermission.Controllers.GroupBy(x => GetControllerName(x.FullName)).ToList();
            foreach (var controller in versionOfControllers)
            {
                var actions = controller.ToList().SelectMany(x => x.Actions).DistinctBy(x => x.Name).ToList();
                var firstController = controller.First();
                var existsPagePermission =
                        await context.Permission
                       .SingleOrDefaultAsync(x => x.Title == firstController.Name && x.NameSpace == firstController.FullName);

                var pagePermission = existsPagePermission ??
                                     Permission.Create(firstController.Type, firstController.Url, firstController.Name,
                                     firstController.FullName, ++priority, PermissionLevelType.Page, tabPermission.Id);
                pagePermission.SetParents([]);

                foreach (var action in actions)
                {
                    var isExistsAction = await context.Permission.AnyAsync(x => x.Title == action.Name && x.NameSpace == action.FullNames.FirstOrDefault());
                    if (isExistsAction)
                        continue;

                    var pagePermissionAction =
                            Permission.Create(action.Type, action.Url, action.Name, action.FullNames.FirstOrDefault() ?? "",
                                              ++priority, PermissionLevelType.Action, pagePermission.Id);

                    if (!pagePermission.Children.Any(x => x.Title == pagePermissionAction.Title && x.NameSpace == pagePermissionAction.NameSpace))
                        pagePermission.Children.Add(pagePermissionAction);
                }

                if (!tabPermission.Children.Any(x => x.Title == pagePermission.Title && x.NameSpace == pagePermission.NameSpace))
                    tabPermission.Children.Add(pagePermission);
            }
            if (existsTabPermission is null)
                context.Permission.Add(tabPermission);
            else
                context.Permission.Update(tabPermission);

            priority++;
        }
        await context.SaveChangesAsync();

        var permissionIds =
                context.ChangeTracker
               .Entries<Permission>()
               .SelectMany(x => x.Properties)
               .Where(x => x.Metadata.Name == "Id")
               .Select(x => Convert.ToInt32(x.CurrentValue))
               .ToList();

        return permissionIds;

        static string GetControllerName(string fullName)
           => fullName.Split(".").Last();
    }

    private async Task SeedUsersAsync(int adminRoleId)
    {
        var adminUserRole = UserRole.Create(adminRoleId);

        List<User> users =
            [
                User.Create("hamidmohammadnian@gmail.com", GenderType.Male, "09307653782", "Hamid", "Mohammadnian", "09307653782",
                            SecurityUtility.GetSha256Hash("admin"), Guid.NewGuid().ToString())
                    .SetUserRoles([ adminUserRole ]),

                User.Create("hosseinojaq@gmail.com", GenderType.Male, "09383109379", "Hossein", "Ojaq", "09383109379",
                            SecurityUtility.GetSha256Hash("admin"), Guid.NewGuid().ToString())
                    .SetUserRoles([ adminUserRole ])
            ];

        foreach (var user in users)
        {
            if (!await context.User.AnyAsync(x => x.UserName == user.UserName))
                context.User.Add(user);
        }
        await context.SaveChangesAsync();
    }

    private async Task SeedRolePermissionsAsync(List<int> permissionIds, int adminRoleId)
    {
        var rolePermissionIds =
             context.RolePermission
            .Where(x => permissionIds.Contains(x.Id))
            .AsNoTracking()
            .Select(x => x.Id)
            .ToList();

        permissionIds =
            permissionIds
            .Where(x => !rolePermissionIds
            .Contains(x))
            .ToList();

        var rolePermissionsForAdmin = permissionIds.Select(x => RolePermission.Create(adminRoleId, (PermissionType)x!)).ToList();
        context.RolePermission.AddRange(rolePermissionsForAdmin);
        await context.SaveChangesAsync();
    }

}