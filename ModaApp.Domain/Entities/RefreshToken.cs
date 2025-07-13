using ModaApp.Domain.Entities;

namespace ModaApp.Domain.Entities;

public class RefreshToken
{
    public long Id { get; private set; }
    public string JwtId { get; private set; } = default!;
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public DateTime ExpiredDateOnUtc { get; private set; }
    public bool Used { get; private set; }
    public bool Invalidated { get; private set; }
    public int UserId { get; private set; }


    public User User { get; private set; } = default!;


    public static RefreshToken Create(int userId, string refreshToken, DateTime expiredDate)
        => new()
        {
            UserId = userId,
            JwtId = refreshToken,
            ExpiredDateOnUtc = expiredDate
        };
    public void Invalidate()
    {
        Invalidated = true;
    }
    public void UsedToken()
    {
        Used = true;
    }
}