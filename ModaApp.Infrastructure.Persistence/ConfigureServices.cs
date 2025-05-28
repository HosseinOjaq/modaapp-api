using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ModaApp.Application.Common.Contracts;
using Microsoft.Extensions.DependencyInjection;
using ModaApp.Infrastructure.Persistence.SeedData;


namespace ModaApp.Infrastructure.Persistence;

public static class ConfigureServices
{
    public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext<ModaAppDbContext>(
            options => options.UseSqlServer(configuration
            .GetConnectionString("ModaAppDbContext")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Services
        services.AddScoped<ISeedService, SeedService>();

        return services;
    }
}