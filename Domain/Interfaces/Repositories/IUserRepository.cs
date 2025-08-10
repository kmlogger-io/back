using System;
using System.Linq.Expressions;
using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByEmail(string email, CancellationToken cancellationToken);
    Task UpdateRefreshToken(User user, CancellationToken cancellationToken);
    Task RevokeRefreshToken(string email, CancellationToken cancellationToken);
}