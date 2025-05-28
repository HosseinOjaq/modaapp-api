using Autofac;
using ModaApp.Api.Modules;
using Autofac.Extensions.DependencyInjection;

namespace ModaApp.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddAutofactServiceProviderAndInterceptors(this WebApplicationBuilder applicationBuilder)
    {
        applicationBuilder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        applicationBuilder.Host.ConfigureContainer<ContainerBuilder>
                     (builder => builder.RegisterModule(new RepositoryModule()));
    }
}