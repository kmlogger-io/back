namespace Domain.Interfaces.Repositories.Cold;

public interface IDbCommit
{
    Task Commit(CancellationToken cancellationToken);
}