using System.Net;
using ModaApp.Common.Models;
using ModaApp.Application.Models.Services;
using ModaApp.Application.Common.Contracts;

namespace ModaApp.Api.Middlewares;

/// <summary>
/// Check Is Blocked Token.
/// </summary>
/// <param name="accountingService"></param>
/// <param name="next"></param>
public class BlockTokenControlMiddleware
    (IAccountingService accountingService, RequestDelegate next)
{
    public Task InvokeAsync(HttpContext context)
    {
        var auth = context.Request?.Headers.Authorization.ToString();
        var token = auth?.Replace("Bearer ", string.Empty, StringComparison.CurrentCultureIgnoreCase);
        if (string.IsNullOrEmpty(token))
            return next(context);

        var isBlocked = accountingService.IsTokenBlocked(new CheckTokenRequestDto(token));
        if (isBlocked.IsSuccess && isBlocked.Result is true)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json";
            context.Response
                   .WriteAsync(System.Text.Json.JsonSerializer
                   .Serialize(ErrorModel.Create("InvalidToken", "Invalid Token")));
            return Task.CompletedTask;
        }

        return next(context);
    }
}