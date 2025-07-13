using ModaApp.Domain.Entities;

namespace ModaApp.Domain.Repositories;

public interface IRefreshTokenRepository
{
    void Add(RefreshToken refreshToken);
    Task<RefreshToken?> GetByToken(string token);
}