using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace ModaApp.WebFramework.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
        }
    }
}