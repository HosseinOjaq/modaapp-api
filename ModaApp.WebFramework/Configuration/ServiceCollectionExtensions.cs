using Asp.Versioning;
using ElmahCore.Mvc;
using ElmahCore.Sql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModaApp.WebFramework.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddElmahCore(this IServiceCollection services, IConfiguration configuration, string elmahPath)
        {
            services.AddElmah<SqlErrorLog>(options =>
            {
                options.Path = elmahPath;
                options.ConnectionString = configuration.GetConnectionString("Elmah");
            });
        }
        public static void AddCustomApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
        }

        public static void AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("CustomCors", builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
            ));
        }
    }
}