using ModaApp.Common.Models;

namespace ModaApp.Application.Common.Contracts;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Save all entities in to database.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OperationResult> SaveChangesAsync(CancellationToken cancellationToken = default);
}