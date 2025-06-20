using Domain.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class DbCommit(KmloggerDbContext context) : IDbCommit
    {
        public async Task Commit(CancellationToken cancellationToken)
            => await context.SaveChangesAsync(cancellationToken);
    }
}