using System;
using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IRoleRepository : IBaseRepository<Role>
{
    Task<List<Role>> GetAllByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default);
}