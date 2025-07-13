using ModaApp.Domain.Entities;
using System.Linq.Expressions;

namespace ModaApp.Domain.Repositories;

public interface IUserRepository
{
    void Add(User user);
    Task<bool> VerifyRepeatEmailAsync(string email);
    Task<bool> VerifyRepeatPhoneAsync(string phoneNumber);
    Task<bool> AnyAsync(Expression<Func<User, bool>> expression);
    Task<User?> FindAsync(int userId, CancellationToken cancellationToken = default);
    Task<User?> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    Task<User?> FindByUserNamePasswordAsync(string userName, string hashPassword, CancellationToken cancellationToken = default);
}