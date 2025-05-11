using System;

namespace Domain.Interfaces.Repositories.Hot;

public interface IDbCommit
{
    Task Commit(CancellationToken cancellationToken);
}
