using Autofac;
using System.Reflection;
using Autofac.Extras.DynamicProxy2;

namespace ModaApp.Api.Modules;

/// <summary>
/// Set Cache Interceptor For Repositories
/// </summary>
public class RepositoryModule : Autofac.Module
{
    /// <inheritdoc/>
    protected override void Load(ContainerBuilder builder)
    {
        var infrastructurePersistence = Assembly.Load("ModaApp.Infrastructure.Persistence");
        builder.RegisterAssemblyTypes(infrastructurePersistence)
          .Where(x => x.Name.EndsWith("Repository"))
          .AsImplementedInterfaces()
          .InstancePerLifetimeScope()
          .EnableClassInterceptors();
    }
}