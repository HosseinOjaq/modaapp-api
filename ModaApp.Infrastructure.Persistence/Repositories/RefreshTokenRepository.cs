using ModaApp.Domain.Entities;
using ModaApp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ModaApp.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository
    (ModaAppDbContext context)
    : IRefreshTokenRepository
{
    public void Add(RefreshToken refreshToken)
    {
        context.RefreshToken.Add(refreshToken);
    }

    public async Task<RefreshToken?> GetByToken(string token)
    {
        return await context.RefreshToken
            .SingleOrDefaultAsync(x => x.JwtId == token && !x.Invalidated && !x.Used);
    }
}