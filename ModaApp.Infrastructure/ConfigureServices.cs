using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModaApp.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}