using ModaApp.Application.Common.Services;
using ModaApp.Application.Common.Contracts;
using ModaApp.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ModaApp.Application.Services.Implementations;

namespace ModaApp.Application;

public static class ConfigureServices
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IAccountingService, AccountingService>();
        return services;
    }
}