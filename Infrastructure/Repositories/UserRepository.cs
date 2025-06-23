using System;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(KmloggerDbContext context)
    : BaseRepository<User>(context), IUserRepository
{

    public async Task<User?> GetByEmail(string email, CancellationToken cancellationToken) =>
        await context.Set<User>().AsNoTracking()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Email.Address.Equals(email), cancellationToken);
    public async Task UpdateRefreshToken(User user, CancellationToken cancellationToken)
    {
        var existingUser = await context.Users.FindAsync(user.Id);
        existingUser?.AssignRefreshToken(user.RefreshToken, user.RefreshTokenExpiryTime.Value);
    }
    
    public async Task RevokeRefreshToken(string email, CancellationToken cancellationToken)
    {
        var user = await GetByEmail(email, cancellationToken);
        user?.ClearRefreshToken();
    }
}
