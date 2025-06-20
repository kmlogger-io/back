using System;
using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<bool> Authenticate(User user, CancellationToken cancellationToken);
    Task<User> ActivateUserAsync(string email, Guid token, CancellationToken cancellationToken);
    Task<User?> GetByEmail(string email, CancellationToken cancellationToken);
    Task UpdateRefreshToken(User user, CancellationToken cancellationToken);
    Task RevokeRefreshToken(string email, CancellationToken cancellationToken);
}