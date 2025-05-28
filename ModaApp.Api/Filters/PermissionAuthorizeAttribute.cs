using ModaApp.Api.Attributes;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ModaApp.Common.Extensions;
using ModaApp.Common.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using ModaApp.Application.Services.Interfaces;

namespace ModaApp.Api.Filters;

/// <summary>
/// Check User Has Permission
/// </summary>
public class PermissionAuthorizeAttribute(IPermissionService permissionService)
    : AuthorizeAttribute, IAuthorizationFilter
{
    public async void OnAuthorization(AuthorizationFilterContext context)
    {
        var actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
        var actionRequiredToAuthorize =
            actionDescriptor.EndpointMetadata.OfType<AuthorizeAttribute>().Any();

        if (actionRequiredToAuthorize)
        {
            if (!(context.HttpContext.User.Identity?.IsAuthenticated ?? false))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var claimsIdentity = context.HttpContext.User.Identity as ClaimsIdentity;
            if (claimsIdentity!.Claims?.Any() != true)
            {
                context.Result = new NotFoundObjectResult("درخواست نامعتبر");
                return;
            }

            var actionRequiredToCheckingPermission =
                !actionDescriptor.EndpointMetadata.OfType<NoPermissionAttribute>().Any();

            if (actionRequiredToCheckingPermission)
            {
                var userId = claimsIdentity.GetUserId<int>();
                var permissionAttribute =
                            actionDescriptor
                            .MethodInfo
                            .GetAttribute<ActionInfoAttribute>();
                var hasPermissionResult = permissionService.HasUserPermissionAsync(userId, permissionAttribute!.PermissionType);

                if (!hasPermissionResult.Result!.HasPermission)
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
        }
    }
}