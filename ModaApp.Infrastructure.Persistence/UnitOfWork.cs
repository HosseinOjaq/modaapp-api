using ModaApp.Common.Models;
using Microsoft.Extensions.Logging;
using ModaApp.Application.Common.Contracts;

namespace ModaApp.Infrastructure.Persistence;

public class UnitOfWork
    (ModaAppDbContext context, ILogger<UnitOfWork> logger)
    : IUnitOfWork
{
    private readonly ModaAppDbContext context = context;

    public async Task<OperationResult> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return OperationResult.Success();
        }
        catch (Exception exception)
        {
            logger.LogCritical(exception, exception.Message);
            return OperationResult.Fail(exception.Message);
        }
    }
    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
    public async ValueTask DisposeAsync()
    {
        await context.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}