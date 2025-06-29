using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class RoleRepository(KmloggerDbContext context)
    : BaseRepository<Role>(context),
     IRoleRepository
{
    public async Task<List<Role>> GetAllByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    {
        if (!ids.Any())
            return new List<Role>();
            
        return await context.Set<Role>()
            .Where(r => ids.Contains(r.Id))
            .ToListAsync(cancellationToken);
    }
}
