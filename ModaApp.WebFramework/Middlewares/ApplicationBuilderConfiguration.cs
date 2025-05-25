using Microsoft.AspNetCore.Builder;
using ModaApp.WebFramework.Middlewares;
using Microsoft.AspNetCore.HttpOverrides;

namespace Edition.WebFramework.Middlewares;

public static class ApplicationBuilderConfiguration
{
    public static void UserCustomeForwardedHeaders(this IApplicationBuilder app)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
            //KnownProxies = { IPAddress.Parse("IP-Of-HaProxy") },
            //KnownNetworks = { }
        });
    }

    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
}