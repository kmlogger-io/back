using System;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Repositories.Cold;
using Infrastructure.Data;
using Infrastructure.Data.Cold;
using Infrastructure.Data.Hot;
using Microsoft.EntityFrameworkCore;

using DbCommitCold = Domain.Interfaces.Repositories.Cold.IDbCommit;
using DbCommitHot = Domain.Interfaces.Repositories.Hot.IDbCommit;

namespace Infrastructure.Repositories.Cold
{
    public class DbCommit(ColdDbContext context) : DbCommitCold
    {
        public async Task Commit(CancellationToken cancellationToken)
            => await context.SaveChangesAsync(cancellationToken);
    }
}

namespace Infrastructure.Repositories.Hot
{
    public class DbCommit(HotDbContext context) : DbCommitHot
    {
        public async Task Commit(CancellationToken cancellationToken)
            => await context.SaveChangesAsync(cancellationToken);
    }
}